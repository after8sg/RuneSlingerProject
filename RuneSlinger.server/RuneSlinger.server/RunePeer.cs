﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using log4net;
using System.Collections.Generic;
using System.Linq;
using System;
using RuneSlinger.Base;
using NHibernate.Linq;
using RuneSlinger.server.Entities;
using RuneSlinger.server.ValueObjects;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Bson;
using RuneSlinger.Base.Abstract;
using RuneSlinger.server.CommandHandlers;
using RuneSlinger.Base.Commands;
using RuneSlinger.server.Abstract;
using Autofac;
using System.Reflection;
using NHibernate;
using RuneSlinger.server.Components;
using RuneSlinger.server.Command;

namespace RuneSlinger.server
{
    public class RunePeer : PeerBase, INetworkedSession
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RunePeer));
        private readonly Application _application;
        
        private readonly IContainer _container;
        private readonly ISerializer _serializer;

        public SessionAuth Auth { get; private set; }
        public Registry Registry { get; private set; }

        public RunePeer(Application application,InitRequest initRequest,IContainer container) : base(initRequest.Protocol,initRequest.PhotonPeer)
        {
            Auth = new SessionAuth();
            _application = application;
            _serializer = _container.Resolve<ISerializer>();
            _container = container;
            Registry = new Registry();

            log.InfoFormat("Peer created at {0}:{1}", initRequest.RemoteIP, initRequest.RemotePort);

            //SendEvent(new EventData(
            //    0, 
            //    new Dictionary<byte, object> 
            //    {
            //        {0,"Peer created"} 
            //    }), 
            //    new SendParameters 
            //    { 
            //        Unreliable = false 
            //    });
        }

        //private void Publish(IEvent @event)
        //{
        //    log.InfoFormat("Publish {0}", @event.GetType());
        //    SendEvent(
        //        new EventData(
        //            (byte)RuneEventCode.SendEvent, 
        //            new Dictionary<byte, object>
        //            {
        //                {(byte) RuneEventCodeParameter.EventType, @event.GetType().AssemblyQualifiedName},
        //                {(byte) RuneEventCodeParameter.EventBytes,SerializeBSON(@event)}
        //            })
        //            , new SendParameters { Unreliable = false });
        //}

        public void Authenticate(SessionAuth auth)
        {
            Auth = auth;
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            //invoke when client tell server to do something
            //if (operationRequest.OperationCode != 0)
            //{
            //    log.WarnFormat("Peer sent unknown opcode: {0}", operationRequest.OperationCode);
            //    return;
            //}
            //var message = (string)operationRequest.Parameters[0];
            //log.DebugFormat("Get message from client: {0}", message);

            //var eventData = new EventData(0, new Dictionary<byte, object> { { 0, message } });
            //var parameters = new SendParameters { Unreliable = false };
            //foreach (var peer in _application.Peers.Where(t => t != this))
            //{
            //    peer.SendEvent(eventData,parameters);
            //}


            //user registration
            // user authentication
            // sending a message
            if (operationRequest.OperationCode != (byte)RuneOperationCode.DispatchCommand)
            {
                SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.Invalid), sendParameters);
                log.WarnFormat("Peer sent unknown operation code: {0}", operationRequest.OperationCode);
                return;
            }

            //using (var session = _application.OpenSession())
            using (var lifeTimeScope = _container.BeginLifetimeScope())
            {

                
                //using (var trans = session.BeginTransaction())
                using (var trans = lifeTimeScope.Resolve<ISession>().BeginTransaction())
                {
                    try
                    {
                        var events = lifeTimeScope.Resolve<IEventPublisher>();
                        //implementing command factory design
                        var commandContext = new CommandContext();

                        var commandType = (string)operationRequest.Parameters[(byte)RuneOperationCodeParameter.CommandType];
                        var commandBytes = (byte[])operationRequest.Parameters[(byte)RuneOperationCodeParameter.CommandBytes];
                        var commandId = operationRequest.Parameters[(byte)RuneOperationCodeParameter.CommandId];

                        var command = (ICommand)_serializer.Deserialize(commandBytes, Type.GetType(commandType));
                        
                        /// using IOC method
                        object handler;
                        if (!lifeTimeScope.TryResolve(typeof(ICommandHandler<>).MakeGenericType(new[] { command.GetType() }), out handler))
                        {
                            SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.Invalid), sendParameters);
                            log.WarnFormat("Peer sent unknown command: {0}", commandType);
                            trans.Rollback();
                            return;
                        }
                        
                        handler.GetType().GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public).Invoke(
                            handler,
                            new object[]
                            {
                                this,commandContext,command
                            }
                            );
                        ///end of using IOC method
                        ///

                        ///-------------------------------------------------------------------------------
                        ///using traditional method as compared to above
                        ///
                        //var loginCommand = command as LoginCommand;
                        //var registerCommand = command as RegisterCommand;
                        //var sendlobbyMessageCommand = command as SendLobbyMessageCommand;
                        //var challengeCommand = command as ChallengePlayerCommand;
                        //var respondToChallengeCommand = command as RespondToChallengeCommand;
                        //var placeRuneCommand = command as PlaceRuneCommand;

                        //if (loginCommand !=  null)
                        //{
                        //    (new LoginHandler(session,_application)).Handle(this,commandContext, loginCommand);
                        //}
                        //else if (registerCommand != null)
                        //{
                        //    (new RegisterHandler(session, _application)).Handle(this, commandContext, registerCommand);
                        //}
                        //else if (sendlobbyMessageCommand != null)
                        //{
                        //    (new SendLobbyMessageHandler( _application)).Handle(this, commandContext, sendlobbyMessageCommand);
                        //}
                        //else if (challengeCommand != null)
                        //{
                        //    (new ChallengePlayerHandler(session,_application)).Handle(this, commandContext, challengeCommand);
                        //}
                        //else if (respondToChallengeCommand != null)
                        //{
                        //    (new RespondToChallengeHandler(session,_application)).Handle(this, commandContext, respondToChallengeCommand);
                        //}
                        //else if (placeRuneCommand != null)
                        //{
                        //    (new PlaceRuneHandler()).Handle(this, commandContext, placeRuneCommand);
                        //}
                        //else
                        //{
                        //    SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.Invalid), sendParameters);
                        //    log.WarnFormat("Peer sent unknown command: {0}", commandType);
                        //    trans.Rollback();
                        //    return;
                        //}
                        ///
                        ///-------------------------------------------------------------
                        ///

                        var parameters = new Dictionary<byte, object>();
                        if (commandContext.Response != null)
                        {
                            parameters[(byte)RuneOperationResponseParameter.CommandResponse] = _serializer.Serialize(commandContext.Response);
                        }

                        parameters[(byte)RuneOperationResponseParameter.OperationErrors] = _serializer.Serialize(commandContext.OperationErrors);
                        parameters[(byte)RuneOperationResponseParameter.PropertyErrors] = _serializer.Serialize(commandContext.PropertyErrors);
                        parameters[(byte)RuneOperationResponseParameter.CommandId] = commandId;

                        SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.CommandDispatched, parameters), sendParameters);
                        trans.Commit();

                        //publish only if transaction is successful
                        events.Commit();
                    }
                    catch (Exception ex)
                    {
                        SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.FatalError), sendParameters);
                        trans.Rollback();
                        log.ErrorFormat("Error processing operation {0} : {1}", operationRequest.OperationCode, ex);
                    }
                }
            }

            

        }

        private void SendMessage(NHibernate.ISession session, string message)
        {
            
        }

        
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            //photon telling client has disconnected
            log.InfoFormat("Peer disconnected {0}:{1}", reasonCode, reasonDetail);

            using (var lifetime = _container.BeginLifetimeScope())
            {
                lifetime.Resolve < ICommandHandler<DisconnectCommand>>().Handle(this, new CommandContext(), new DisconnectCommand());
            }

            _application.DestroyPeer(this);
        }

        //private void SendSuccess()
        //{
        //    SendOperationResponse(new OperationResponse((byte)RuneOperationResponse.Success), new SendParameters { Unreliable = false });
        //}

        //private void SendError(string message)
        //{
        //    SendOperationResponse(
        //        new OperationResponse(
        //            (byte)RuneOperationResponse.Error, 
        //            new Dictionary<byte,object>
        //            {
        //                {(byte)RuneOperationResponseParameter.ErrorMessage, message}
        //            }
        //            )
        //       , new SendParameters
        //        {
        //            Unreliable = false
        //        });
        //}

    }
}
