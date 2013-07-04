using UnityEngine;
using Assets.Code.ValueObjects;
using Assets.Code;

public class RuneGO : MonoBehaviour
{
    private Rune _rune;
    private RuneGameGO _game;
    public void Initialize(RuneGameGO game,Rune rune)
    {
        _rune = rune;
        _game = game;
        
        GameObjectHelpers.CreateSprite(gameObject, RuneTextureMap.GetRuneAssigner(rune.Type, rune.RuneTypeIndex));
    }
}

