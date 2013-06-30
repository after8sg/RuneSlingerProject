using UnityEngine;
using RuneSlinger.Base.Events;
using RuneSlinger.Base.ValueObjects;
using Assets.Code;
using Assets.Code.Abstract;
using System.Collections.Generic;
using RuneSlinger.Base.Commands;
using System.Linq;

public class LobbyGO :
    MonoBehaviour,
    IEventHandler<SessionJoinedLobbyEvent>,
    IEventHandler<SessionLeftLobbyEvent>,
    IEventHandler<LobbyMessageSendEvent>,
    IEventHandler<ChallengeCreatedEvent>,
    IEventHandler<ChallengeRespondedToEvent>,
    IEventHandler<SessionJoinedGameEvent>
{
    enum LobbyState
    {
        Default,
        IsChallenged,
        IsChallenger,
        Waiting
    }
    private Dictionary<uint, LobbySession> _sessions;
    private HashSet<uint> _sessionsInGame;
    private List<string> _messages;
    private string _message;
    private Vector2 _messageScrollPosition;
    private Vector2 _usersInLobbyScrollPosition;
    private LobbyState _lobbyState;
    private LobbySession _otherChallengeUser;
    
    public void Awake()
    {
        NetworkManager.Instance.RegisterEventHandler(this);
        _sessionsInGame = new HashSet<uint>();
        _sessions = new Dictionary<uint, LobbySession>();
        _messages = new List<string>();
        _message = "";
        _lobbyState = LobbyState.Default;
        
    }

    public void OnDestroy()
    {
        NetworkManager.Instance.UnRegisterEventHandler(this);
    }

    public void AddSession(LobbySession session)
    {
        _sessions.Add(session.Id, session);
    }

    public void Handle(SessionJoinedLobbyEvent @event)
    {
        AddMessage(string.Format("{0} entered the lobby", @event.Session.Username));
        AddSession(@event.Session);
    }

    public void Handle(SessionLeftLobbyEvent @event)
    {
        var session = _sessions[@event.UserId];
        AddMessage(string.Format("{0} left the lobby", session.Username));
        _sessions.Remove(@event.UserId);
    }

    public void Handle(LobbyMessageSendEvent @event)
    {
        var session = _sessions[@event.UserId];
        AddMessage(string.Format("{0}: {1}", session.Username, @event.Message));

    }

    public void Handle(ChallengeCreatedEvent @event)
    {
        _lobbyState = LobbyState.IsChallenged;
        _otherChallengeUser = _sessions[@event.ChallengerUserId];
    }

    public void Handle(ChallengeRespondedToEvent @event)
    {
        if (@event.Response == ChallengeResponse.Rejected)
        {
            _lobbyState = LobbyState.Default;
            AddMessage(string.Format("{0} rejected your challenge", _otherChallengeUser.Username));
        }
        else if (@event.Response == ChallengeResponse.Aborted)
        {
            _lobbyState = LobbyState.Default;
            AddMessage("Challenge was aborted");
        }
        
    }

    public void Handle(SessionJoinedGameEvent @event)
    {
        if (@event.UserIds.Contains(GameManager.Instance.UserId))
            return;
        
        foreach (var userId in @event.UserIds)
                _sessionsInGame.Add(userId);
        
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            if (_lobbyState == LobbyState.Default)
                DefaultStateGUI();
            else if (_lobbyState == LobbyState.IsChallenged)
                IsChallengedStateGUI();
            else if (_lobbyState == LobbyState.IsChallenger)
                IsChallengerStateGUI();
            else if (_lobbyState == LobbyState.Waiting)
                WaitingStateGUI();
        }
        GUILayout.EndHorizontal();

        GUI.FocusControl("message");
    }

    private void WaitingStateGUI()
    {
        GUILayout.Label("Stuff and things! (waiting state)");
    }

    private void IsChallengerStateGUI()
    {
        GUILayout.Label(string.Format("Waiting for response from {0}", _otherChallengeUser.Username));
    }

    private void IsChallengedStateGUI()
    {
        GUILayout.Label(string.Format("You have been challenged by {0}", _otherChallengeUser.Username));

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Accept"))
            {
                RespondToChallenge(ChallengeResponse.Accepted);
            }

            if (GUILayout.Button("Reject"))
            {
                RespondToChallenge(ChallengeResponse.Rejected);
            }

        }
        GUILayout.EndHorizontal();
    }

    private void RespondToChallenge(ChallengeResponse response)
    {
        _lobbyState = LobbyState.Waiting;
        NetworkManager.Instance.Dispatch(new RespondToChallengeCommand(response), serverResponse =>
            {
                if (serverResponse.IsValid && response != ChallengeResponse.Rejected)
                    return;

                _lobbyState = LobbyState.Default;
                AddMessage(string.Format("You rejected {0}'s challenge",_otherChallengeUser.Username));
            });
    }

    private void DefaultStateGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(150));
        {
            _usersInLobbyScrollPosition = GUILayout.BeginScrollView(_usersInLobbyScrollPosition);
            {
                foreach (var session in _sessions)
                {
                    if (!_sessionsInGame.Contains(session.Value.Id))
                    {
                        if (GUILayout.Button(session.Value.Username))
                            ChallengePlayer(session.Value);
                    }
                    else
                        GUILayout.Label(session.Value.Username);
                }
                    
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUILayout.Width(500));
        {
            _messageScrollPosition = GUILayout.BeginScrollView(_messageScrollPosition);
            {
                foreach (var message in _messages)
                    GUILayout.Label(message);
            }
            GUILayout.EndScrollView();

            GUI.SetNextControlName("message");
            _message = GUILayout.TextField(_message);
            if ((Event.current.keyCode == KeyCode.Return || GUILayout.Button("Send"))
                && !string.IsNullOrEmpty(_message)
                )
            {
                SendLobbyMessage(_message);
                _message = "";
            }

        }
        GUILayout.EndVertical();
    }

    private void ChallengePlayer(LobbySession challenged)
    {
        _lobbyState = LobbyState.IsChallenger;
        _otherChallengeUser = challenged;

        NetworkManager.Instance.Dispatch(new ChallengePlayerCommand(challenged.Id), response =>
            {
                if (response.IsValid)
                    return;

                _lobbyState = LobbyState.Default;
                AddMessage("Challenge Failed");

            });
    }

    private void SendLobbyMessage(string message)
    {
        NetworkManager.Instance.Dispatch(new SendLobbyMessageCommand(message));
    }

    private void AddMessage(string message)
    {
        _messages.Add(message);
        _messageScrollPosition = new Vector2(_messageScrollPosition.x, float.MaxValue);
    }



    
}

