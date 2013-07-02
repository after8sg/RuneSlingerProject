using UnityEngine;
using Assets.Code.ValueObjects;
using Assets.Code;

public class RuneSlotGO : MonoBehaviour
{
    private uint _x;
    private uint _y;
    private RuneSlot _slot;
    private RuneGameGO _game;

    public void Initialize(RuneGameGO game, uint x, uint y, RuneSlot runeSlot)
    {
        _x = x;
        _y = y;
        _slot = runeSlot;
        _game = game;

        //transform.localScale = new Vector3(game.Screen.GridCellSize.x, game.Screen.GridCellSize.y, 1);
        //transform.localPosition = new Vector3(x * game.Screen.GridCellSize.x, y * game.Screen.GridCellSize.y, -2);

        GameObjectHelpers.CreateSprite(gameObject, new BasicTextureAssigner("TileSlotBackground"));
    }
}

