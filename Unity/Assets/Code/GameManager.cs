using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class GameManager : MonoBehaviour , IPhotonPeerListener
{

    private PhotonPeer _photonPeer;
    private string _message;
    private List<string> _messages;
	// Use this for initialization
	public void Start () 
    {

        _photonPeer = new PhotonPeer(this, ConnectionProtocol.Udp);
        if (!_photonPeer.Connect("127.0.0.1:5055", "RuneSlinger"))
            Debug.LogError("Could not connect to photon!!");

        _message = "";
        _messages = new List<string>();
	}
	
	// Update is called once per frame
	public void Update () 
    {

        _photonPeer.Service();
	}

    public void OnGUI()
    {
        _message = GUI.TextField(new Rect(0, 0, 200, 40), _message);
        if (GUI.Button(new Rect(0, 45, 100, 40), "Send Message"))
        {
            SendServer(_message);
            _message = "";
        }

        GUI.Label(new Rect(0, 90, 300, 500), string.Join("\n", _messages.ToArray()));
    }

    private void SendServer(string message)
    {
        //opcustom will cause photon to send message to server
        _photonPeer.OpCustom(
            0,
            new Dictionary<byte, object>
            {
                {0,message}
            },
            true);

    }

    public void OnApplicationQuit()
    {
        _photonPeer.Disconnect();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
    }

    public void OnEvent(EventData eventData)
    {
        //Debug.Log("Event: " + eventData.Parameters[0].ToString());
        _messages.Add(eventData.Parameters[0].ToString());
    }
}

