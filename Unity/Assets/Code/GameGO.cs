using UnityEngine;
using Assets.Code;

public class GameGO : MonoBehaviour
{
    public void Awake()
    {
        NetworkManager.Instance.RegisterEventHandler(this);
    }

    public void OnDestroy()
    {
        NetworkManager.Instance.UnRegisterEventHandler(this);
    }

    public void OnGUI()
    {
        GUILayout.Label("IN GAME !!!");
    }

}

