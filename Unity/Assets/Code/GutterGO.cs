using UnityEngine;
using Assets.Code;
using Assets.Code.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;


public class GutterGO : MonoBehaviour
{
    private GutterLocation _location;
    private RuneGameGO _runeGame;
    private List<RuneGO> _runes;
    private GutterInnerGO _gutterInnerGo;
    private bool _isPanning;
    private bool _canPan;
    private bool _isMovingRune;
    private float _minY;
    private float _maxY;
    private RuneGO _selectedRune;
    private Vector3 _selectedRuneStartingPosition;
    private RuneSlotGO _hoveredSlot;
    private IGameCommands _gameCommands;
    private float _heightOfInner;

    public void Initialize(RuneGameGO game, GutterLocation gutterLocation, IGameCommands gameCommands)
    {
        _gameCommands = gameCommands;
        _runeGame = game;
        _location = gutterLocation;
        _runes = new List<RuneGO>();
        _gutterInnerGo = GameObjectFactory.Create<GutterInnerGO>("Inner");
        _gutterInnerGo.transform.parent = transform;

        GameObjectHelpers.CreateSprite(gameObject, new BasicTextureAssigner(_location == GutterLocation.Left ? "GutterLeftBg" : "GutterRightBg"));
        transform.position = new Vector3(_location == GutterLocation.Left ? 0 : _runeGame.Screen.Resolution.x - _runeGame.Screen.GridCellSize.x,0,-20);
        transform.localScale = new Vector3(_runeGame.Screen.GridCellSize.x, _runeGame.Screen.Resolution.y,1);
    }

    public RuneGO AddRune(Rune rune)
    {
        var runeGO = GameObjectFactory.Create<RuneGO>(string.Format("Rune - {0}", rune.Id));
        

        
        runeGO.transform.parent = _gutterInnerGo.transform;
        runeGO.transform.localScale = new Vector3(1, _runeGame.Screen.GridCellSize.y / transform.localScale.y, 1);
        runeGO.transform.localPosition = GetRunePositionByIndex(_runes.Count);
        runeGO.Initialize(this,_runeGame,rune);
        _runes.Add(runeGO);

        return runeGO;
    }

    private Vector3 GetRunePositionByIndex(int index)
    {
        return new Vector3(
            0,
            index * _runeGame.Screen.GridCellSize.y / transform.localScale.y,
            -2
            );
    }

    public void StartGame()
    {
        RecalculatePanningBounds();
        CenterInner();

        _isMovingRune = _isPanning = false;
        
    }

    private void CenterInner()
    {
        _gutterInnerGo.transform.localPosition = new Vector3(
            0,
          -((_heightOfInner / 2) - (_runeGame.Screen.Resolution.y / 2)) / transform.localScale.y,
            0);
    }

    private void RecalculatePanningBounds()
    {
        _heightOfInner = (_runes.Count * _runeGame.Screen.GridCellSize.y);
        _maxY = 0;
        _minY = -(_heightOfInner - _runeGame.Screen.Resolution.y) / transform.localScale.y;
        _canPan = _heightOfInner > _runeGame.Screen.Resolution.y;

        if (!_canPan)
        {
            CenterInner(); 
            return;
        }
        
        if (_gutterInnerGo.transform.localPosition.y > _maxY)
            _gutterInnerGo.transform.localPosition = new Vector3(_gutterInnerGo.transform.localPosition.x, _maxY, _gutterInnerGo.transform.localPosition.z);
        else if (_gutterInnerGo.transform.localPosition.y < _minY)
            _gutterInnerGo.transform.localPosition = new Vector3(_gutterInnerGo.transform.localPosition.x, _minY, _gutterInnerGo.transform.localPosition.z);
        

    }

    public void Update()
    {
        if (_isMovingRune)
        {
            if (Input.GetMouseButtonUp(0))
            {
                DropSelectedRune();
            }
            else
                MoveSelectedRune();
        }
        else if (_isPanning)
        {
            if (Input.GetMouseButtonUp(0))
                _isPanning = false;
            else
            {
                if (IsWithinGutterX(Input.mousePosition.x))
                    PanGutter();
                else if (_selectedRune != null)
                    PickUpSelectedRune();
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var mouseX = Input.mousePosition.x;
            if (IsWithinGutterX(mouseX))
            {
                var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
                var hitRune = hits.SelectMany(t => t.collider.gameObject.GetComponents<MonoBehaviour>()).OfType<RuneGO>().SingleOrDefault();
                _selectedRune = hitRune;
                _isPanning = true;
            }
            
        }
    }

    private void PickUpSelectedRune()
    {
        _isPanning = false;
        _isMovingRune = true;
        _selectedRuneStartingPosition = _selectedRune.transform.position;
    }

    private void DropSelectedRune()
    {
        if (!_isMovingRune) return;

        if (_hoveredSlot != null)
        {
            _hoveredSlot.SetHighlight(null);
            _gameCommands.PlaceRune(_selectedRune.Rune, _hoveredSlot.Slot);
        }
        
        _selectedRune.transform.position = _selectedRuneStartingPosition;
        _isMovingRune = false;
        _selectedRune = null;
        _hoveredSlot = null;

        
    }

    public void PanGutter()
    {
        if (!_canPan) return;

        var yDelta = Input.GetAxis("Mouse Y") * 15;

        var newY = _gutterInnerGo.transform.localPosition.y + yDelta / transform.localScale.y;
        if (newY < _minY)
            newY = _minY;
        else if (newY > _maxY)
            newY = _maxY;
        _gutterInnerGo.transform.localPosition = new Vector3(
            _gutterInnerGo.transform.localPosition.x,
           newY ,
            _gutterInnerGo.transform.localPosition.z
            );
    }

    private bool IsWithinGutterX(float mouseX)
    {
        return _location == GutterLocation.Left ? mouseX < _runeGame.Screen.GridCellSize.x :
            mouseX > _runeGame.Screen.Resolution.x - _runeGame.Screen.GridCellSize.x;
    }

    private void MoveSelectedRune()
    {
        //var xDelta = Input.GetAxis("Mouse X") * 15;
        //var yDelta = Input.GetAxis("Mouse Y") * 15;

        //_selectedRune.transform.position = new Vector3(
        //    _selectedRune.transform.position.x + xDelta,
        //    _selectedRune.transform.position.y + yDelta,
        //    -30
        //    );

        _selectedRune.transform.position = new Vector3(
            Input.mousePosition.x - (_runeGame.Screen.GridCellSize.x / 2),
            Input.mousePosition.y - (_runeGame.Screen.GridCellSize.y / 2),
            -30
            );

        var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        var hitSlot = hits.SelectMany(t => t.collider.gameObject.GetComponents<MonoBehaviour>()).OfType<RuneSlotGO>().FirstOrDefault(r => !r.IsPlaced);

        if (hitSlot == _hoveredSlot) return;

        if (_hoveredSlot != null)
            _hoveredSlot.SetHighlight(null);

        _hoveredSlot = hitSlot;

        if (hitSlot != null)
        {
            _hoveredSlot.SetHighlight(SlotHighlightType.Hover);
        }
        
    }

    public void ResetSelectedRune()
    {
        _hoveredSlot = null;
        DropSelectedRune();
    }

    public void Place(RuneGO runeGO)
    {
        _hoveredSlot = null;
        _isMovingRune = false;
        _selectedRune = null;

        Destroy(runeGO.gameObject);

        var indexOfRune = _runes.IndexOf(runeGO);
        _runes.Remove(runeGO);

        for (var i = indexOfRune; i < _runes.Count; i++)
        {
            var rune = _runes[i];
            rune.transform.position = GetRunePositionByIndex(i);
        }

        RecalculatePanningBounds();
    }


}

