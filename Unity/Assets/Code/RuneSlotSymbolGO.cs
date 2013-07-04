
using UnityEngine;
using Assets.Code.ValueObjects;
using Assets.Code;
public class RuneSlotSymbolGO : MonoBehaviour
{
    
    public void Initialize(RuneGameGO game, RuneSlot slot)
    {
        GameObjectHelpers.CreateSprite(gameObject, RuneTextureMap.GetSlotAssigner(slot.Type,slot.RuneTypeIndex));
    }

}

