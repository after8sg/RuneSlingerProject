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

public class NetworkManager : MonoBehaviour, IPhotonPeerListener
{
    //singleton 
    public static NetworkManager Instance { get; private set; }
    private PhotonPeer _photonPeer;
    private JsonSerializer _jsonSerializer;
    private Action<Dictionary<byte,object>> _commandCallback;

    // Use this for initialization
    public void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("You cannot create more than one!");
            return;
        }
        Instance = this;
        _photonPeer = new PhotonPeer(this, ConnectionProtocol.Udp);
        if (!_photonPeer.Connect("127.0.0.1:5055", "RuneSlinger"))
            Debug.LogError("Could not connect to photon!!");

        _jsonSerializer = new JsonSerializer();
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

    public void Dispatch<TCommand>(TCommand command,Action<CommandContext> action)
    {
        _commandCallback = parameters =>
            {
                action(new CommandContext(
                                    DeserializeBSON<IDictionary<string, IEnumerable<string>>>((byte[])parameters[(byte)RuneOperationResponseParameter.PropertyErrors]),
                                    DeserializeBSON<IEnumerable<string>>((byte[])parameters[(byte)RuneOperationResponseParameter.OperationErrors],true)
                                    )
                    );
            };
        DispatchInternal(command);
    }
        
    public void Dispatch<TResponse>(ICommand<TResponse> command, Action<CommandContext<TResponse>> action) where TResponse : ICommandResponse
    {
        _commandCallback = parameters =>
        {
            var response = default(TResponse);
            if (parameters.ContainsKey((byte)RuneOperationResponseParameter.CommandResponse))
                response = DeserializeBSON<TResponse>((byte[])parameters[(byte)RuneOperationResponseParameter.CommandResponse]);

            action(new CommandContext<TResponse>(
                                response,
                                DeserializeBSON<IDictionary<string, IEnumerable<string>>>((byte[])parameters[(byte)RuneOperationResponseParameter.PropertyErrors]),
                                DeserializeBSON<IEnumerable<string>>((byte[])parameters[(byte)RuneOperationResponseParameter.OperationErrors],true)
                                )
                );
        };
        DispatchInternal(command);
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
            _commandCallback(operationResponse.Parameters);

        }
    }
    
    public void OnEvent(EventData eventData)
    {
        
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        
    }

    private void DispatchInternal<TCommand>(TCommand command)
    {
        var parameters = new Dictionary<byte, object>();
        parameters[(byte)RuneOperationCodeParameter.CommandType] = command.GetType().AssemblyQualifiedName;
        parameters[(byte)RuneOperationCodeParameter.CommandBytes] = SerializeBSON(command);

        _photonPeer.OpCustom((byte)RuneOperationCode.DispatchCommand, parameters, true);
    }

    private object DeserializeBSON(byte[] bytes,Type type)
    {
        using (var ms = new MemoryStream(bytes))
        {
            return _jsonSerializer.Deserialize(new BsonReader(ms), type);
        }
    }

    //generic
    private TObject DeserializeBSON<TObject>(byte[] bytes, bool isArray = false)
    {
        using (var ms = new MemoryStream(bytes))
        {
            return _jsonSerializer.Deserialize<TObject>(new BsonReader(ms,isArray,DateTimeKind.Local));
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

