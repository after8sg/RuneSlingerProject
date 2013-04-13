using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using RuneSlinger.Base;
using RuneSlinger.Base.Commands;

public class GameManager : MonoBehaviour
{

    enum GameManagerState
    {
        Form,
        Sending,
        Error,
        LoggedIn
    }

    private string _displayUsername;
    private string _registerEmail;
    private string _registerPassword;
    private string _loginEmail;
    private string _loginPassword;
    private string _username;
    private string _error;
    private GameManagerState _state;

    public void Start()
    {
        
        _registerEmail = "";
        _registerPassword = "";
        _username = "";
        _loginEmail = "";
        _loginPassword = "";
        _state = GameManagerState.Form;
    }

    public void Update()
    {
        
    }

    //public void OnApplicationQuit()
    //{
    //    _photonPeer.Disconnect();
    //}

    public void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Width(800), GUILayout.Height(600));

        if (_state == GameManagerState.Form || _state == GameManagerState.Error)
        {
            GUILayout.Label("Rune Game Registration");

            if (_state == GameManagerState.Error)
                GUILayout.Label(string.Format("error: {0}", _error));

            GUILayout.Label("Username");
            _username = GUILayout.TextField(_username);

            GUILayout.Label("Email");
            _registerEmail = GUILayout.TextField(_registerEmail);

            GUILayout.Label("Password");
            _registerPassword = GUILayout.TextField(_registerPassword);

            if (GUILayout.Button("Register"))
            {
                Register(_username, _registerPassword, _registerEmail);
            }

            GUILayout.Label("Rune Game Login");

            GUILayout.Label("Email");
            _loginEmail = GUILayout.TextField(_loginEmail);

            GUILayout.Label("Password");
            _loginPassword = GUILayout.TextField(_loginPassword);

            if (GUILayout.Button("Login"))
                Login(_loginEmail, _loginPassword);
        }
        else if (_state == GameManagerState.Sending)
        {
            GUILayout.Label("Sending ...");
        }
        else if (_state == GameManagerState.LoggedIn)
        {
            GUILayout.Label("Success: " + _displayUsername) ;
        }

        GUILayout.EndHorizontal();

        //_message = GUI.TextField(new Rect(0, 0, 200, 40), _message);
        //if (GUI.Button(new Rect(0, 45, 100, 40), "Send Message"))
        //{
        //    SendServer(_message);
        //    _message = "";
        //}

        //GUI.Label(new Rect(0, 90, 300, 500), string.Join("\n", _messages.ToArray()));
    }

    private void Login(string _email, string _password)
    {
        _state = GameManagerState.Sending;
        NetworkManager.Instance.Dispatch(new LoginCommand(_email, _password), response =>
            {
                if (response.IsValid)
                {
                    _state = GameManagerState.LoggedIn;
                    _displayUsername = response.Response.Username;
                }
                else
                {
                    _state = GameManagerState.Error;
                    _error = response.ToErrorString();
                }
            }
        );

        //_photonPeer.OpCustom(
        //    (byte)RuneOperationCode.Login,
        //    new Dictionary<byte, object>
        //        {
        //            { (byte)RuneOperationCodeParameter.Email,_email} ,
        //            { (byte)RuneOperationCodeParameter.Password,_password}

        //        },
        //        true
        //        );
    }

    private void Register(string _username, string _password, string _email)
    {
        //_state = GameManagerState.Sending;
        //_photonPeer.OpCustom(
        //    (byte)RuneOperationCode.Register,
        //    new Dictionary<byte, object>
        //        {
        //            { (byte)RuneOperationCodeParameter.Username,_username} ,
        //            { (byte)RuneOperationCodeParameter.Email,_email} ,
        //            { (byte)RuneOperationCodeParameter.Password,_password}

        //        },
        //        true
        //        );

        _state = GameManagerState.Sending;
        NetworkManager.Instance.Dispatch(new RegisterCommand(_email, _username, _password), response =>
            {
                if (response.IsValid)
                {
                    _state = GameManagerState.LoggedIn;
                    _displayUsername = _username;
                }
                else
                {
                    _state = GameManagerState.Error;
                    _error = response.ToErrorString();
                }
            });
    }

    //private void SendServer(string message)
    //{
    //    //opcustom will cause photon to send message to server
    //    _photonPeer.OpCustom(
    //        0,
    //        new Dictionary<byte, object>
    //        {
    //            {0,message}
    //        },
    //        true);

    //}

    

    
}

