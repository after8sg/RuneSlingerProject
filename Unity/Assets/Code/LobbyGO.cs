using UnityEngine;
using RuneSlinger.Base.Events;
using RuneSlinger.Base.ValueObjects;
using Assets.Code;
using Assets.Code.Abstract;
using System.Collections.Generic;
using RuneSlinger.Base.Commands;

public class LobbyGO :
    MonoBehaviour,
    IEventHandler<SessionJoinedLobbyEvent>,
    IEventHandler<SessionLeftLobbyEvent>,
    IEventHandler<LobbyMessageSendEvent>
{

    private Dictionary<uint, LobbySession> _sessions;
    private List<string> _messages;
    private string _message;
    private Vector2 _messageScrollPosition;
    private Vector2 _usersInLobbyScrollPosition;

    public void Awake()
    {
        NetworkManager.Instance.RegisterEventHandler(this);
        _sessions = new Dictionary<uint, LobbySession>();
        _messages = new List<string>();
        _message = "";
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

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical(GUILayout.Width(150));
            {
                _usersInLobbyScrollPosition = GUILayout.BeginScrollView(_usersInLobbyScrollPosition);
                {
                    foreach (var session in _sessions)
                        GUILayout.Label(session.Value.Username);
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
                if ( (Event.current.keyCode == KeyCode.Return || GUILayout.Button("Send"))
                    && !string.IsNullOrEmpty(_message)
                    )
                {
                    SendLobbyMessage(_message);
                    _message = "";
                }
                
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        GUI.FocusControl("message");
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

