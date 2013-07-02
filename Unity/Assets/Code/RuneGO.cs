using UnityEngine;
using Assets.Code.ValueObjects;

public class RuneGO : MonoBehaviour
{
    private Rune _rune;

    public void Initialize(Rune rune)
    {
        _rune = rune;
    }
}

