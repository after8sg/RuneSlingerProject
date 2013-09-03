using UnityEngine;
using ExitGames.Client.Photon;
using RuneSlinger.Base.Abstract;
using Assets.Code;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using RuneSlinger.Base;
using Newtonsoft.Json.Bson;
using System.Reflection;
using Assets.Code.Abstract;

namespace Assets.Code
{
    public class NetworkManager : IPhotonPeerListener
    {
        private class EventHandlerRegistration
        {
            public object Handler { get; private set; }
            public MethodInfo Method { get; private set; }

            public EventHandlerRegistration(object handler, MethodInfo method)
            {
                Handler = handler;
                Method = method;
            }
        }

        //singleton 
        private static NetworkManager _instance;
        public static NetworkManager Instance { get { return _instance ?? (_instance = new NetworkManager()); } }

        private readonly PhotonPeer _photonPeer;
        private readonly JsonSerializer _jsonSerializer;
        private readonly Dictionary<Guid, Action<OperationResponse>> _commandCallbacks;
        private readonly Dictionary<Type, List<EventHandlerRegistration>> _eventHandlerRegistrations;

        private NetworkManager()
        {

            _jsonSerializer = new JsonSerializer();
            _commandCallbacks = new Dictionary<Guid, Action<OperationResponse>>();
            _eventHandlerRegistrations = new Dictionary<Type, List<EventHandlerRegistration>>();

            _photonPeer = new PhotonPeer(this, ConnectionProtocol.Udp);
            if (!_photonPeer.Connect("127.0.0.1:5055", "RuneSlinger"))
                Debug.LogError("Could not connect to photon!!");


        }

        // Update is called once per frame
        public void Update()
        {
            _photonPeer.Service();
        }

        public void OnApplicationQuit()
        {
            _photonPeer.Disconnect();
        }

        public void RegisterEventHandler(object eventHandlers)
        {
            foreach (var inter in eventHandlers.GetType().GetInterfaces())
            {
                //class Test : IEventHandler<LoginEvent>

                //inter != IEventHandler<>
                //inter == IEventHandler<LoginEvent>
                //here inter refers to the second instead of the first definition
                if (!inter.IsGenericType || inter.GetGenericTypeDefinition() != typeof(IEventHandler<>))
                    continue;

                //the first argument refers to LoginEvent
                var eventType = inter.GetGenericArguments()[0];

                //check if handler exists, if not create it
                List<EventHandlerRegistration> handlers;
                if (!_eventHandlerRegistrations.TryGetValue(eventType, out handlers))
                    handlers = _eventHandlerRegistrations[eventType] = new List<EventHandlerRegistration>();

                var methodToInvoke = inter.GetMethods(BindingFlags.Instance | BindingFlags.Public)[0];
                handlers.Add(new EventHandlerRegistration(eventHandlers, methodToInvoke));
            }
        }

        public void UnRegisterEventHandler(object eventHandlers)
        {
            foreach (var eventItem in _eventHandlerRegistrations.Values)
            {
                eventItem.RemoveAll(t => t.Handler == eventHandlers);
            }
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            DispatchInternal(command, Guid.NewGuid());
        }

        public void Dispatch<TCommand>(TCommand command, Action<CommandContext> action)
        {
            var commandId = Guid.NewGuid();
            _commandCallbacks.Add(commandId, serverResponse =>
                {
                    var parameters = serverResponse.Parameters;
                    var propertyErrors = DeserializeBSON<IDictionary<string, IEnumerable<string>>>((byte[])parameters[(byte)RuneOperationResponseParameter.PropertyErrors]);
                    var operationErrors = DeserializeBSON<IEnumerable<string>>((byte[])parameters[(byte)RuneOperationResponseParameter.OperationErrors], true);

                    action(new CommandContext(propertyErrors, operationErrors));
                });


            DispatchInternal(command, commandId);
        }

        public void Dispatch<TResponse>(ICommand<TResponse> command, Action<CommandContext<TResponse>> action) where TResponse : ICommandResponse
        {

            var commandId = Guid.NewGuid();
            _commandCallbacks.Add(commandId, serverResponse =>
                {
                    var parameters = serverResponse.Parameters;
                    var response = default(TResponse);
                    if (parameters.ContainsKey((byte)RuneOperationResponseParameter.CommandResponse))
                        response = DeserializeBSON<TResponse>((byte[])parameters[(byte)RuneOperationResponseParameter.CommandResponse]);

                    action(new CommandContext<TResponse>(
                                        response,
                                        DeserializeBSON<IDictionary<string, IEnumerable<string>>>((byte[])parameters[(byte)RuneOperationResponseParameter.PropertyErrors]),
                                        DeserializeBSON<IEnumerable<string>>((byte[])parameters[(byte)RuneOperationResponseParameter.OperationErrors], true)
                                        )
                        );
                });


            DispatchInternal(command, commandId);
        }

        public void DebugReturn(DebugLevel level, string message)
        {
        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            var responseCode = (RuneOperationResponse)operationResponse.OperationCode;
            if (responseCode == RuneOperationResponse.FatalError)
                Debug.LogError("You broke the serve");
            else if (responseCode == RuneOperationResponse.Invalid)
                Debug.LogError("Invalid command!");
            else if (responseCode == RuneOperationResponse.CommandDispatched)
            {
                var commandId = new Guid((byte[])operationResponse.Parameters[(byte)RuneOperationResponseParameter.CommandId]);

                Action<OperationResponse> responseCallback;
                if (_commandCallbacks.TryGetValue(commandId,out responseCallback))
                {
                    _commandCallbacks[commandId](operationResponse);
                    _commandCallbacks.Remove(commandId);
                }

            }
        }

        public void OnEvent(EventData eventData)
        {
            if (eventData.Code != (byte)RuneEventCode.SendEvent)
                throw new InvalidOperationException("Unknown event received from server");

            var type = (string)eventData.Parameters[(byte)RuneEventCodeParameter.EventType];
            var bytes = (byte[])eventData.Parameters[(byte)RuneEventCodeParameter.EventBytes];

            var eventType = Type.GetType(type, true);
            var @event = (IEvent)DeserializeBSON(bytes, eventType);

            List<EventHandlerRegistration> handlers;
            if (!_eventHandlerRegistrations.TryGetValue(eventType, out handlers))
                return;

            foreach (var handler in handlers)
                handler.Method.Invoke(handler.Handler, new object[] { @event });



        }

        public void OnStatusChanged(StatusCode statusCode)
        {

        }

        private void DispatchInternal<TCommand>(TCommand command, Guid commandId)
        {
            var parameters = new Dictionary<byte, object>();
            parameters[(byte)RuneOperationCodeParameter.CommandType] = command.GetType().AssemblyQualifiedName;
            parameters[(byte)RuneOperationCodeParameter.CommandBytes] = SerializeBSON(command);
            parameters[(byte)RuneOperationCodeParameter.CommandId] = commandId.ToByteArray();

            _photonPeer.OpCustom((byte)RuneOperationCode.DispatchCommand, parameters, true);
        }

        private object DeserializeBSON(byte[] bytes, Type type, bool isArray = false)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return _jsonSerializer.Deserialize(new BsonReader(ms, isArray, DateTimeKind.Local), type);
            }
        }

        //generic
        private TObject DeserializeBSON<TObject>(byte[] bytes, bool isArray = false)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return _jsonSerializer.Deserialize<TObject>(new BsonReader(ms, isArray, DateTimeKind.Local));
            }
        }

        private byte[] SerializeBSON(object obj)
        {
            using (var ms = new MemoryStream())
            {
                _jsonSerializer.Serialize(new BsonWriter(ms), obj);
                return ms.ToArray();
            }
        }

    }

}