using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRoomManager : MonoBehaviour
{
    private NetworkedClient client;
    private TMP_InputField roomNameInputField;
    private Button createRoomButton;
    private Button leaveRoomButton;
    [SerializeField]private GameObject createGameRoomUI;
    [SerializeField]private GameObject gameRoomUI;

    private void Start()
    {
        client = GameObject.Find("NetworkedClient").GetComponent<NetworkedClient>();
        roomNameInputField = GameObject.Find("CreateGameRoomInputField").GetComponent<TMP_InputField>();
        createRoomButton = GameObject.Find("CreateGameRoomButton").GetComponent<Button>();
        leaveRoomButton = GameObject.Find("LeaveRoomButton").GetComponent<Button>();
        createGameRoomUI.gameObject.SetActive(false);
        gameRoomUI.gameObject.SetActive(false);

        createRoomButton.onClick.AddListener(CreateNewRoom);
        leaveRoomButton.onClick.AddListener(LeaveRoom);
    }

    private void Update()
    {
        switch (StateManager.Instance.state)
        {
            case StateManager.GameState.CREATEGAMEROOM:
                SetUp();
                break;
            case StateManager.GameState.GAMEROOM:
                createGameRoomUI.SetActive(false);
                gameRoomUI.SetActive(true);
                break;
        }
    }

    private void SetUp()
    {
        createGameRoomUI.SetActive(true);
        gameRoomUI.gameObject.SetActive(false);
    }

    private void CreateNewRoom()
    {
        client.SendMessageToHost(Signifiers.CreateRoomSignifier + "," + roomNameInputField.GetComponent<TMP_InputField>().text);
    }

    private void LeaveRoom()
    {
        client.SendMessageToHost(Signifiers.LeaveRoomSignifier.ToString());
        StateManager.Instance.state = StateManager.GameState.CREATEGAMEROOM;
    }
}
