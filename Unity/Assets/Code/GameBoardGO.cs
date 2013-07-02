using UnityEngine;
using Assets.Code.ValueObjects;
using System;
using System.Collections.Generic;
using Assets.Code;

public class GameBoardGO : MonoBehaviour
{
    private RuneGameGO _game;
    private List<RuneSlotGO> _runeSlots;

    public void Initialize(RuneGameGO game)
    {
        _game = game;
        _runeSlots = new List<RuneSlotGO>();

        transform.localScale = new Vector3(game.Screen.GridCellSize.x * game.Width, game.Screen.GridCellSize.y * game.Height, 1);
        
        GameObjectHelpers.CreateSprite(gameObject, new BasicTextureAssigner("Background"));
    }

    public void AddSlot(uint x, uint y, RuneSlot runeSlot)
    {
        if (x >= _game.Width || y >= _game.Height)
            throw new ArgumentException("Slot must placed in the game board");

        var runeSlotGO = GameObjectFactory.Create<RuneSlotGO>(string.Format("Rune Slot - {0}", runeSlot.Id));
        runeSlotGO.transform.localScale = new Vector3(_game.Screen.GridCellSize.x, _game.Screen.GridCellSize.y, 1);
        runeSlotGO.transform.position = new Vector3(x * _game.Screen.GridCellSize.x, y * _game.Screen.GridCellSize.y, -2);

        runeSlotGO.transform.parent = transform;

        runeSlotGO.Initialize(_game, x, y, runeSlot);

        _runeSlots.Add(runeSlotGO);

        
    }

    public void Start()
    {
        transform.position = new Vector3(_game.Screen.GridCellSize.x, 0, 1);
    }
}

