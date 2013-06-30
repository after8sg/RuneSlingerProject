using UnityEngine;
using System;
using RuneSlinger.Base.Events;
using Assets.Code.Abstract;
using System.Linq;
using Assets.Code;

public class GameManager : 
    MonoBehaviour, 
    IEventHandler<JoinLobbyEvent>,
    IEventHandler<SessionJoinedGameEvent>
{
    public static GameManager Instance {get; private set;}

    public uint UserId { get; set; }

    private GameObject _loginGo;
    private LobbyGO _lobbyGo;
    private GameGO _gameGo;

    public void Start()
    {
        
        if (Instance != null)
            throw new InvalidOperationException("Cannot create more than one game manager");

        Instance = this;
        NetworkManager.Instance.RegisterEventHandler(this);

        _loginGo = new GameObject("Login");
        _loginGo.AddComponent<LoginGO>();
    }

    public void OnDestroy()
    {
        NetworkManager.Instance.UnRegisterEventHandler(this);
    }

    public void Update()
    {
        NetworkManager.Instance.Update();
    }

    public void OnApplicationQuit()
    {
        NetworkManager.Instance.OnApplicationQuit();
    }

    public void Handle(JoinLobbyEvent @event)
    {

        Destroy(_loginGo);

        var go = new GameObject("Lobby");
        _lobbyGo = go.AddComponent<LobbyGO>();

        foreach (var session in @event.Sessions)
            _lobbyGo.AddSession(session);
    }

    public void Handle(SessionJoinedGameEvent @event)
    {
        if (!@event.UserIds.Contains(UserId))
            return;

        _lobbyGo.gameObject.SetActive(false);

        var go = new GameObject("Game");
        _gameGo = go.AddComponent<GameGO>();
    }

    //public void Publish(IEvent @event)
    //{

    //    //can reimplement below into class as similar to how commands are handle
    //    var joinLobbyEvent = @event as JoinLobbyEvent;
    //    var lobbyJoinedEvent = @event as SessionJoinedLobbyEvent;
    //    var lobbyLeftEvent = @event as SessionLeftLobbyEvent;
    //    var messageSendEvent = @event as LobbyMessageSendEvent;

    //    if (joinLobbyEvent != null)
    //    {
    //        foreach (var session in joinLobbyEvent.Sessions)
    //            _messages.Add(string.Format("{0} - {1} is in lobby", session.Username, session.Id));
    //    }
    //    else if (lobbyJoinedEvent != null)
    //    {
    //        _messages.Add(string.Format("{0} - {1} entered the lobby", lobbyJoinedEvent.Session.Username, lobbyJoinedEvent.Session.Id));
    //    }
    //    else if (lobbyLeftEvent != null)
    //    {
    //        _messages.Add(string.Format("{0} left the lobby", lobbyLeftEvent.UserId));
    //    }
    //    else if (messageSendEvent != null)
    //    {
    //        _messages.Add(string.Format("{0} said {1}", messageSendEvent.UserId, messageSendEvent.Message));
    //    }
    //}

    //public void OnApplicationQuit()
    //{
    //    _photonPeer.Disconnect();
    //}

    
    //private void SendServer(string message)
    //{
    //    //opcustom will cause photon to send message to server
    //    _photonPeer.OpCustom(
    //        0,
    //        new Dictionary<byte, object>
    //        {
    //            {0,message}
    //        },
    //        true);

    //}


}

