using UnityEngine;
using Assets.Code.ValueObjects;
using System;
using System.Collections.Generic;
using Assets.Code;

public class GameBoardGO : MonoBehaviour
{
    private RuneGameGO _game;
    private Dictionary<uint,RuneSlotGO> _runeSlots;

    private bool isPanning;
    private bool canPanX, canPanY;
    private float minX, minY, maxX, maxY;

    public void Initialize(RuneGameGO game)
    {
        _game = game;
        _runeSlots = new Dictionary<uint,RuneSlotGO>();

        transform.localScale = new Vector3(game.Screen.GridCellSize.x * game.BoardWidth, game.Screen.GridCellSize.y * game.BoardHeight, 1);
        
        transform.position = new Vector3(
            -(transform.localScale.x / 2) + (game.Screen.Resolution.x / 2)
            , -(transform.localScale.y / 2) + (game.Screen.Resolution.y / 2)
            , 1
            );
        GameObjectHelpers.CreateSprite(gameObject, new BasicTextureAssigner("Background"));

        maxX = _game.Screen.GridCellSize.x;
        minX = -(transform.localScale.x - _game.Screen.GridCellSize.x) + (_game.Screen.Resolution.x - (_game.Screen.GridCellSize.x * 2));

        maxY = 0;
        minY = -(transform.localScale.y - _game.Screen.GridCellSize.y) + (_game.Screen.Resolution.y - _game.Screen.GridCellSize.y);

        if (game.BoardWidth > game.ScreenWidth - 2)
            canPanX = true;

        if (game.BoardHeight > game.Screen.GridSize.y)
            canPanY = true;
    }

    public void AddSlot(uint x, uint y, RuneSlot runeSlot)
    {
        if (x >= _game.BoardWidth || y >= _game.BoardHeight)
            throw new ArgumentException("Slot must placed in the game board");

        var runeSlotGO = GameObjectFactory.Create<RuneSlotGO>(string.Format("Rune Slot - {0}", runeSlot.Id));
        runeSlotGO.transform.localScale = new Vector3(_game.Screen.GridCellSize.x, _game.Screen.GridCellSize.y, 1);
        runeSlotGO.transform.parent = transform;
        runeSlotGO.transform.localPosition = new Vector3(
           ( x * _game.Screen.GridCellSize.x) / transform.localScale.x
            , (y * _game.Screen.GridCellSize.y) / transform.localScale.y
            , -2
            );

        

        runeSlotGO.Initialize(_game, x, y, runeSlot);

        _runeSlots.Add(runeSlot.Id,runeSlotGO);

        
    }

    public void StartGame()
    {
        isPanning = false;
    }

    public void Update()
    {
        if (isPanning)
        {
            if (Input.GetMouseButtonUp(0))
                isPanning = false;
            else
                PanBoard();

        }
        else if (Input.GetMouseButtonDown(0))
        {
            var mouseX = Input.mousePosition.x;
            if (mouseX > _game.Screen.GridCellSize.x && mouseX < _game.Screen.Resolution.x - _game.Screen.GridCellSize.x)
                isPanning = true;
        }
    }

    private void PanBoard()
    {
        var xDelta = Input.GetAxis("Mouse X") * 15;
        var yDelta = Input.GetAxis("Mouse Y") * 15;

        var newX = transform.localPosition.x;
        if (canPanX)
        {
            newX += xDelta;
            if (newX < minX)
                newX = minX;
            else if (newX > maxX)
                newX = maxX;
        }

        var newY = transform.localPosition.y;
        if (canPanY)
        {
            newY += yDelta;
            if (newY < minY)
                newY = minY;
            else if (newY > maxY)
                newY = maxY;
        }

        transform.localPosition = new Vector3(
            newX,
            newY,
            transform.localPosition.z);
    }


    public void PlaceRune(uint slotId)
    {
        _runeSlots[slotId].Place();
    }
}

