using UnityEngine;
using Assets.Code;
using Assets.Code.ValueObjects;
using System.Collections.Generic;

public class GutterGO : MonoBehaviour
{
    private GutterLocation _location;
    private RuneGameGO _runeGame;
    private List<RuneGO> _runes;
    private GutterInnerGO _gutterInnerGo;

    public void Initialize(RuneGameGO game,GutterLocation gutterLocation)
    {
        _runeGame = game;
        _location = gutterLocation;
        _runes = new List<RuneGO>();
        _gutterInnerGo = GameObjectFactory.Create<GutterInnerGO>("Inner");
        _gutterInnerGo.transform.parent = transform;

        GameObjectHelpers.CreateSprite(gameObject, new BasicTextureAssigner(_location == GutterLocation.Left ? "GutterLeftBg" : "GutterRightBg"));
        transform.position = new Vector3(_location == GutterLocation.Left ? 0 : _runeGame.Screen.Resolution.x - _runeGame.Screen.GridCellSize.x,0,-20);
        transform.localScale = new Vector3(_runeGame.Screen.GridCellSize.x, _runeGame.Screen.Resolution.y,1);
    }

    public void AddRune(Rune rune)
    {
        var runeGO = GameObjectFactory.Create<RuneGO>(string.Format("Rune - {0}", rune.Id));
        

        _runes.Add(runeGO);
        runeGO.transform.parent = _gutterInnerGo.transform;
        runeGO.transform.localScale = new Vector3(1, _runeGame.Screen.GridCellSize.y / transform.localScale.y, 1);
        runeGO.transform.localPosition = new Vector3(
            0,
            _runes.Count * _runeGame.Screen.GridCellSize.y / transform.localScale.y,
            -25
            );
        runeGO.Initialize(_runeGame,rune);
    }

    public void StartGame()
    {
        var heightOfInner = (_runes.Count * _runeGame.Screen.GridCellSize.y);
        _gutterInnerGo.transform.localPosition = new Vector3(
            0,
          -((heightOfInner / 2) - (_runeGame.Screen.Resolution.y / 2)) / transform.localScale.y,
            0);
    }

}

