using UnityEngine;
using Assets.Code;
using Assets.Code.ValueObjects;
using System.Collections.Generic;

public class GutterGO : MonoBehaviour
{
    private GutterLocation _location;
    private RuneGameGO _runeGame;
    private List<RuneGO> _runes;

    public void Initialize(RuneGameGO game,GutterLocation gutterLocation)
    {
        _runeGame = game;
        _location = gutterLocation;
        _runes = new List<RuneGO>();

        GameObjectHelpers.CreateSprite(gameObject, new BasicTextureAssigner(_location == GutterLocation.Left ? "GutterLeftBg" : "GutterRightBg"));
        transform.position = new Vector3(_location == GutterLocation.Left ? 0 : _runeGame.Screen.Resolution.x - _runeGame.Screen.GridCellSize.x,0);
        transform.localScale = new Vector3(_runeGame.Screen.GridCellSize.x, _runeGame.Screen.Resolution.y);
    }

    public void AddRune(Rune rune)
    {
        var runeGO = GameObjectFactory.Create<RuneGO>(string.Format("Rune - {0}", rune.Id));
        runeGO.Initialize(rune);

        _runes.Add(runeGO);
        runeGO.transform.parent = transform;
    }
}

