using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
    private NetworkedClient client;

    // Input Fields UI
    private TMP_InputField[] userInputFields;
    private TMP_InputField usernameInputField;
    private TMP_InputField passwordInputField;
    private TMP_InputField newUsernameInputField;
    private TMP_InputField newPasswordInputField;

    // Button UI
    public Button[] accountsButtons;
    public Button loginButton;
    public Button switchToLoginUIButton;
    public Button switchToNewAccountButton;
    public Button registerAccountButton;

    // GameObject References
    [SerializeField]
    private GameObject newAccountUI;
    [SerializeField]
    private GameObject loginAccountUI;

    private void Start()
    {
        client = GameObject.Find("NetworkedClient").GetComponent<NetworkedClient>();
        userInputFields = GameObject.FindObjectsOfType<TMP_InputField>(true);
        foreach (TMP_InputField inputField in userInputFields)
        {
            if (inputField.name == "UsernameInputField")
            {
                usernameInputField = inputField;
            }
            else if (inputField.name == "PasswordInputField")
            {
                passwordInputField = inputField;
            }
            else if (inputField.name == "NewUsernameInputField")
            {
                newUsernameInputField = inputField;
            }
            else if (inputField.name == "NewPasswordInputField")
            {
                newPasswordInputField = inputField;
            }
        }
        accountsButtons = GameObject.FindObjectsOfType<Button>(true);
        foreach (Button button in accountsButtons)
        {
            if (button.name == "LoginAccountButton" )
            {
                loginButton = button;
            }
            else if (button.name == "SwitchToLoginButton")
            {
                switchToLoginUIButton = button;
            }
            else if (button.name == "SwitchToNewAccountButton")
            {
                switchToNewAccountButton = button;
            }
            else if (button.name == "CreateAccountButton")
            {
                registerAccountButton = button;
            }
        }
        AddClickEventListeners();
    }

    private void Update()
    {
        switch (StateManager.Instance.state)
        {
            case StateManager.GameState.CREATEACCOUNT:
                SwitchNewAccountUI();
                break;
            case StateManager.GameState.LOGIN:
                SwitchLoginAccountUI();
                break;
            case StateManager.GameState.CREATEGAMEROOM:
                DisableAccountUI();
                break;
        }
    }

    private void AddClickEventListeners()
    {
        switchToLoginUIButton.onClick.AddListener(delegate{SetState(1);});
        switchToNewAccountButton.onClick.AddListener(delegate{SetState(0);});
        loginButton.onClick.AddListener(LoginToAccount);
        registerAccountButton.onClick.AddListener(RegisterAccount);
    }

    private void RegisterAccount()
    {
        client.SendMessageToHost(Signifiers.RegisterAccountSignifier.ToString() + "," + newUsernameInputField.GetComponent<TMP_InputField>().text + "," + newPasswordInputField.GetComponent<TMP_InputField>().text);
    }
     
    private void LoginToAccount()
    {
        client.SendMessageToHost(Signifiers.LoginAccountSignifier.ToString() + "," + usernameInputField.GetComponent<TMP_InputField>().text + "," + passwordInputField.GetComponent<TMP_InputField>().text);
    }

    private void SetState(int slot)
    {
        if (slot == 0)
        {
            Debug.Log("Create Account State");
            StateManager.Instance.state = StateManager.GameState.CREATEACCOUNT;
        }
        else if (slot == 1)
        {
            Debug.Log("LogIn state");
            StateManager.Instance.state = StateManager.GameState.LOGIN;
        }
    }

    private void SwitchNewAccountUI()
    {
        if (loginAccountUI.gameObject.activeSelf)
        {
            Debug.Log("Switch to new account");
            newAccountUI.GameObject().SetActive(true);
            loginAccountUI.GameObject().SetActive(false);
        }
    }

    private void SwitchLoginAccountUI()
    {
        if ( newAccountUI.gameObject.activeSelf)
        {
            Debug.Log("Switch to login");
            loginAccountUI.GameObject().SetActive(true);
            newAccountUI.GameObject().SetActive(false);
        }
    }

    private void DisableAccountUI()
    {
        newAccountUI.GameObject().SetActive(false);
        loginAccountUI.GameObject().SetActive(false);
    }
}
