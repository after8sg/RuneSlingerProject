using UnityEngine;
using RuneSlinger.Base.Commands;
using Assets.Code;

public class LoginGO : MonoBehaviour
{
    enum LoginState
    {
        Form,
        Sending,
        Error
    }

    private LoginState _state;
    private string _registerEmail;
    private string _registerPassword;
    private string _registerUsername;
    private string _loginEmail;
    private string _loginPassword;
    private string _error;

    public void Start()
    {
        _registerEmail = "";
        _registerPassword = "";
        _registerUsername = "";
        _loginEmail = "";
        _loginPassword = "";
        _state = LoginState.Form;
    }

    public void OnGUI()
    {
        var currentControl = GUI.GetNameOfFocusedControl();
        var isRegisterFormFocused = currentControl == "registerUsername" || currentControl == "registerEmail" || currentControl == "registerPassword";
        var isLoginFormFocused = currentControl == "loginEmail" || currentControl == "loginPassword";

        GUILayout.BeginVertical(GUILayout.Width(800), GUILayout.Height(600));

        if (_state == LoginState.Form || _state == LoginState.Error)
        {
            GUILayout.Label("Rune Game Registration");

            if (_state == LoginState.Error)
                GUILayout.Label(string.Format("error: {0}", _error));

            GUILayout.Label("Username");
            GUI.SetNextControlName("registerUsername");
            _registerUsername = GUILayout.TextField(_registerUsername);

            GUILayout.Label("Email");
            GUI.SetNextControlName("registerEmail");
            _registerEmail = GUILayout.TextField(_registerEmail);

            GUILayout.Label("Password");
            GUI.SetNextControlName("registerPassword");
            _registerPassword = GUILayout.TextField(_registerPassword);

            if ((isRegisterFormFocused && Event.current.keyCode == KeyCode.Return) || GUILayout.Button("Register"))
            {
                Register(_registerUsername, _registerPassword, _registerEmail);
            }

            GUILayout.Label("Rune Game Login");

            GUILayout.Label("Email");
            GUI.SetNextControlName("loginEmail");
            _loginEmail = GUILayout.TextField(_loginEmail);

            GUILayout.Label("Password");
            GUI.SetNextControlName("loginPassword");
            _loginPassword = GUILayout.TextField(_loginPassword);

            if ((isLoginFormFocused && Event.current.keyCode == KeyCode.Return) ||  GUILayout.Button("Login"))
                Login(_loginEmail, _loginPassword);
        }
        else if (_state == LoginState.Sending)
        {
            GUILayout.Label("Sending ...");
        }
        
        GUILayout.EndHorizontal();

        if (string.IsNullOrEmpty(currentControl))
            GUI.FocusControl("loginEmail");

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
        _state = LoginState.Sending;
        NetworkManager.Instance.Dispatch(new LoginCommand(_email, _password), response =>
        {
            if (response.IsValid)
                return;


            _state = LoginState.Error;
            _error = response.ToErrorString();
            
        }
        );

    }

    private void Register(string _username, string _password, string _email)
    {
        _state = LoginState.Sending;
        NetworkManager.Instance.Dispatch(new RegisterCommand(_email, _username, _password), response =>
        {
            if (response.IsValid)
                return;


            _state = LoginState.Error;
            _error = response.ToErrorString();

        });
    }

}

