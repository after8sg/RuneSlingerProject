using UnityEngine;
using Assets.Code.Abstract;
using Assets.Code;
using System.Collections.Generic;
using Assets.Code.ValueObjects;
using System;
using RuneSlinger.Base.ValueObjects;

public class RuneGameGO : MonoBehaviour
{
    private GameBoardGO _gameBoard;
    private Dictionary<GutterLocation, GutterGO> _gutters;
    private Dictionary<uint, RuneGO> _runeGameObjects;

    private IEnumerable<GameSession> _gameSessions;
    private GameSession _ourSession;
    private IGameCommands _gameCommands;
    public uint ScreenWidth { get; private set; }
    public uint BoardWidth { get; private set; }
    public uint BoardHeight { get; private set; }
    public bool IsPlaying { get; private set; }
    public ScreenConfiguration Screen { get; private set; }

    public void Initialize(IEnumerable<GameSession> gameSessions, GameSession ourSession,IGameCommands gameCommands, uint boardwidth, uint boardheight, uint screenwidth)
    {
        Screen = new ScreenConfiguration(screenwidth);

        IsPlaying = false;
        
        _gameSessions = gameSessions;
        _ourSession = ourSession;
        _gameCommands = gameCommands;
        BoardWidth = boardwidth;
        BoardHeight = boardheight;
        ScreenWidth = screenwidth;

        _gameBoard = GameObjectFactory.Create<GameBoardGO>("Game Board");
        _gameBoard.Initialize(this);

        var leftGutter = GameObjectFactory.Create<GutterGO>("Left Gutter");
        leftGutter.Initialize(this, GutterLocation.Left, _gameCommands);

        var rightGutter = GameObjectFactory.Create<GutterGO>("Right Gutter");
        rightGutter.Initialize(this, GutterLocation.Right, _gameCommands);

        _gutters = new Dictionary<GutterLocation, GutterGO>
        {
            {GutterLocation.Left,leftGutter},
            {GutterLocation.Right,rightGutter}
        };

        _gameBoard.transform.parent = transform;
        leftGutter.transform.parent = transform;
        rightGutter.transform.parent = transform;

        _runeGameObjects = new Dictionary<uint, RuneGO>();
    }

    public void AddSlot(uint x, uint y, RuneSlot runeSlot)
    {
        _gameBoard.AddSlot(x, y, runeSlot);
        
    }

    public void AddRune(GutterLocation gutterLocation, Rune rune)
    {
        var runeGo = _gutters[gutterLocation].AddRune(rune);
        _runeGameObjects.Add(runeGo.Rune.Id, runeGo);
    }

    public void Start()
    {
        IsPlaying = true;
        _gameBoard.StartGame();

        foreach (var gutter in _gutters)
            gutter.Value.StartGame();
    }


    public void RunePlaced(uint runeId, uint slotId)
    {
        _gameBoard.PlaceRune(slotId);

        _runeGameObjects[runeId].Place();
    }

    public void PlacementRejected(uint runeId, uint slotId)
    {
        foreach (var gutter in _gutters)
            gutter.Value.ResetSelectedRune();
    }
}

