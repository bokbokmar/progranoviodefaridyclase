using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayfabHandler : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private string titleID;
    [SerializeField] private string secretKey;
    [Header("Registration")]
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField emailField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmPasswordField;
    [Header("Login")]
    [SerializeField] private TMP_InputField loginUsernameField;
    [SerializeField] private TMP_InputField loginPasswordField;
    [Header("Events")]
    [SerializeField] private UnityEvent onRegisterSuccess;
    [SerializeField] private UnityEvent onLoginSuccess;
    private void Awake()
    {
        PlayFabSettings.TitleId = titleID;
        PlayFabSettings.DeveloperSecretKey = secretKey;
    }

    public void RegisterUser()
    {
        if (passwordField.text != confirmPasswordField.text)
        {
            return;
        }

        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest
        {
            Username = usernameField.text,
            Email = emailField.text,
            Password = passwordField.text,
            RequireBothUsernameAndEmail = true,
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, PlayfabError);
    }

    public void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        onRegisterSuccess.Invoke();
    }

    public void LoginSuccess(LoginResult result)
    {
        onLoginSuccess.Invoke();
    }

    public void PlayfabError(PlayFabError error)
    {
        Debug.Log(error);
    }
    public void LoginUser()
    {
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest

        {

            Username = loginUsernameField.text,
            Password = loginPasswordField.text,
        };

        PlayFabClientAPI.LoginWithPlayFab(request, LoginSuccess, PlayfabError);
    }
}
