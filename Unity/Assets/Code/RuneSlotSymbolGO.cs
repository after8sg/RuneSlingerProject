
using UnityEngine;
using Assets.Code.ValueObjects;
using Assets.Code;
using RuneSlinger.Base.ValueObjects;
public class RuneSlotSymbolGO : MonoBehaviour
{
    
    public void Initialize(RuneGameGO game, RuneSlot slot)
    {
        GameObjectHelpers.CreateSprite(gameObject, RuneTextureMap.GetSlotAssigner(slot.Type,slot.RuneTypeIndex));
    }

}

