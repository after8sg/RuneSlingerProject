using UnityEngine;
using Assets.Code.ValueObjects;
using Assets.Code;
using RuneSlinger.Base.ValueObjects;

public class RuneGO : MonoBehaviour
{
    public Rune Rune { get; private set; }
    private RuneGameGO _game;
    private GutterGO _gutter;

    public void Initialize(GutterGO gutter,RuneGameGO game,Rune rune)
    {
        Rune = rune;
        _game = game;
        _gutter = gutter;

        GameObjectHelpers.CreateSprite(gameObject, RuneTextureMap.GetRuneAssigner(rune.Type, rune.RuneTypeIndex));
        gameObject.AddComponent<BoxCollider>();
    }

    public void Place()
    {
        _gutter.Place(this);
        
    }

}

