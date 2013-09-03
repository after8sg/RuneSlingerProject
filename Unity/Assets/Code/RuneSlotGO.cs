using UnityEngine;
using Assets.Code.ValueObjects;
using Assets.Code;
using RuneSlinger.Base.ValueObjects;

public class RuneSlotGO : MonoBehaviour
{

    public RuneSlot Slot { get; private set; }
    public bool IsPlaced { get; private set; }

    private uint _x;
    private uint _y;
    
    private RuneGameGO _game;
    private RuneSlotSymbolGO _symbolGo;
    private GameObject _highlightGo;

    public void Initialize(RuneGameGO game, uint x, uint y, RuneSlot runeSlot)
    {
        _x = x;
        _y = y;
        Slot = runeSlot;
        _game = game;

        //transform.localScale = new Vector3(game.Screen.GridCellSize.x, game.Screen.GridCellSize.y, 1);
        //transform.localPosition = new Vector3(x * game.Screen.GridCellSize.x, y * game.Screen.GridCellSize.y, -2);

        GameObjectHelpers.CreateSprite(gameObject, new BasicTextureAssigner("TileSlotBackground"));

        _symbolGo = GameObjectFactory.Create<RuneSlotSymbolGO>(string.Format("Symbol: {0}", Slot.Id));
        _symbolGo.transform.parent = transform;
        _symbolGo.transform.localPosition = new Vector3(0, 0, -1);
        _symbolGo.transform.localScale = new Vector3(1, 1, 1);
        _symbolGo.Initialize(game, runeSlot);

        _highlightGo = new GameObject(string.Format("Symbol: {0} Highlight", Slot.Id));
        _highlightGo.transform.parent = transform;
        _highlightGo.transform.localPosition = new Vector3(0, 0, -2);
        _highlightGo.transform.localScale = new Vector3(1, 1, 1);

        GameObjectHelpers.CreateSpriteMesh(_highlightGo);
        _highlightGo.SetActive(false);

        gameObject.AddComponent<BoxCollider>();
    }

    public void SetHighlight(SlotHighlightType? type)
    {
        if (type == null)
            _highlightGo.SetActive(false);
        else
        {
            _highlightGo.SetActive(true);
            SlotHighlightTextureMap.GetHighlightTextureAssigner(type.Value).Assign(_highlightGo.GetComponent<MeshRenderer>(), _highlightGo.GetComponent<MeshFilter>());
        }
    }


    public void Place()
    {
        RuneTextureMap.GetDroppedAssigner(Slot.Type, Slot.RuneTypeIndex).Assign(
            _symbolGo.GetComponent<MeshRenderer>(), _symbolGo.GetComponent<MeshFilter>()
            );

        IsPlaced = true;
    }

}

