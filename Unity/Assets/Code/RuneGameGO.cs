using UnityEngine;
using Assets.Code.Abstract;
using Assets.Code;
using System.Collections.Generic;
using Assets.Code.ValueObjects;
using System;

public class RuneGameGO : MonoBehaviour
{
    private GameBoardGO _gameBoard;
    private Dictionary<GutterLocation, GutterGO> _gutters;

    private IEnumerable<GameSession> _gameSessions;
    private GameSession _ourSession;
    private IGameView _gameView;
    public uint Width { get; private set; }
    public uint Height { get; private set; }
    public bool IsPlaying { get; private set; }
    public ScreenConfiguration Screen { get; private set; }

    public void Initialize(IEnumerable<GameSession> gameSessions, GameSession ourSession,IGameView mockGameView, uint width, uint height)
    {
        Screen = new ScreenConfiguration(width + 2);

        IsPlaying = false;
        
        _gameSessions = gameSessions;
        _ourSession = ourSession;
        _gameView = mockGameView;
        Width = width;
        Height = height;

        _gameBoard = GameObjectFactory.Create<GameBoardGO>("Game Board");
        _gameBoard.Initialize(this);

        var leftGutter = GameObjectFactory.Create<GutterGO>("Left Gutter");
        leftGutter.Initialize(this, GutterLocation.Left);

        var rightGutter = GameObjectFactory.Create<GutterGO>("Right Gutter");
        rightGutter.Initialize(this, GutterLocation.Right);

        _gutters = new Dictionary<GutterLocation, GutterGO>
        {
            {GutterLocation.Left,leftGutter},
            {GutterLocation.Right,rightGutter}
        };

        _gameBoard.transform.parent = transform;
        leftGutter.transform.parent = transform;
        rightGutter.transform.parent = transform;

    }

    public void AddSlot(uint x, uint y, RuneSlot runeSlot)
    {
        _gameBoard.AddSlot(x, y, runeSlot);
        
    }

    public void AddRune(GutterLocation gutterLocation, Rune rune)
    {
        _gutters[gutterLocation].AddRune(rune);
    }

    public void Start()
    {
        IsPlaying = true;
        _gameBoard.Start();
    }

}

