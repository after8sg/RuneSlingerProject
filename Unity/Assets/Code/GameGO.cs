using UnityEngine;
using Assets.Code;
using System.Collections.Generic;
using RuneSlinger.Base.ValueObjects;
using Assets.Code.Abstract;
using RuneSlinger.Base.Events;
using RuneSlinger.Base.Commands;

public class GameGO : MonoBehaviour,
    IEventHandler<GameParametersEstablishedEvent>,
    IEventHandler<GameRuneSlotEstablishedEvent>,
    IEventHandler<GameRuneEstablishedEvent>,
    IEventHandler<GameStartedEvent>,
    IEventHandler<RunePlacedEvent>,
    IGameCommands
{

    private Dictionary<uint, GameSession> _gameSession;
    private RuneGameGO _gameView;
    private GameSession _ourSession;

    public IEnumerable<GameSession> GameSessions { get { return _gameSession.Values; } }

    public void Awake()
    {
        NetworkManager.Instance.RegisterEventHandler(this);
        _gameSession = new Dictionary<uint, GameSession>();
        _gameView = GameObjectFactory.Create<RuneGameGO>("Game View");

    }

    public void OnDestroy()
    {
        NetworkManager.Instance.UnRegisterEventHandler(this);
        Destroy(_gameView.gameObject);
    }

    public void OnGUI()
    {
        
    }


    public void InitaliseSession(IEnumerable<GameSession> sessions,GameSession ourSession)
    {
        foreach (var session in sessions)
            _gameSession.Add(session.UserId, session);
        Debug.Log("game go initialise " + ourSession.UserId);
        _ourSession = ourSession;
    }

    public void Handle(GameParametersEstablishedEvent @event)
    {
        Debug.Log("game parameters event");
        _gameView.Initialize(_gameSession.Values, _ourSession, this, @event.BoardWidth, @event.BoardHeight, 10);
    }

    public void PlaceRune(Rune rune, RuneSlot slot)
    {
        NetworkManager.Instance.Dispatch(new PlaceRuneCommand(rune.Id, slot.Id), response =>
            {
                if (response.Response.WasPlaced)
                    _gameView.RunePlaced(rune.Id, slot.Id);
                else
                    _gameView.PlacementRejected(rune.Id, slot.Id);
            });
    }

    public void Handle(GameRuneSlotEstablishedEvent @event)
    {
        uint x = 0, y = 0;
        foreach (var slot in @event.Slots)
        {
            _gameView.AddSlot(x, y,slot);

            x++;

            if ( x < _gameView.BoardWidth)
            {
                continue;
            }

            x = 0;
            y++;

        }

    }

    public void Handle(GameRuneEstablishedEvent @event)
    {
        var index = 0;
        var halfTotalRunes = _gameView.BoardWidth * _gameView.BoardHeight / 2;

        foreach (var rune in @event.Runes)
        {
            _gameView.AddRune(index > halfTotalRunes ? GutterLocation.Left : GutterLocation.Right, rune);
            index++;
        }

    }

    public void Handle(GameStartedEvent @event)
    {
        _gameView.Start();
    }

    public void Handle(RunePlacedEvent @event)
    {
        _gameView.RunePlaced(@event.RuneId, @event.SlotId);

    }
}

