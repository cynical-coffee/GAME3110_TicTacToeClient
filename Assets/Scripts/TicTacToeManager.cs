using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TicTacToeManager : MonoBehaviour
{
    private NetworkedClient client;
    private Button[] buttonSquares;
    private string player = "";
    private bool isPlayerTurn;
    [SerializeField] private GameObject buttonGrid;

    public enum GameState
    {
        START,
        PLAYERTURN,
        OPPONENTTURN,
        WIN,
        LOSE
    }

    public GameState state;

    private void Start()
    {
        client = GameObject.Find("NetworkedClient").GetComponent<NetworkedClient>();
        buttonSquares = buttonGrid.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttonSquares.Length; i++)
        {
            int k = i;
            buttonSquares[i].onClick.AddListener(delegate { PickSlot(k); });
        }

        state = GameState.START;
        WaitForOpponent();
    }

    private void Update()
    {
        switch (state)
        {
            case GameState.START:
                break;
            case GameState.PLAYERTURN:
                isPlayerTurn = true;
                break;
            case GameState.OPPONENTTURN:
                isPlayerTurn = false;
                break;
        }
        LockBoard(isPlayerTurn);
    }

    private void WaitForOpponent()
    {
        for (int i = 0; i < buttonSquares.Length; i++)
        {
            buttonSquares[i].interactable = false;
        }
    }

    private void PickSlot(int buttonNum)
    {
        buttonSquares[buttonNum].GetComponentInChildren<TextMeshProUGUI>().text = player;
        client.SendMessageToHost(Signifiers.GamePlaySignifier + "," + player + "," + buttonNum.ToString());
        buttonSquares[buttonNum].interactable = false;
        state = GameState.OPPONENTTURN;
    }

    private void LockBoard(bool playerTurn)
    {
        if (playerTurn == false)
        {
            for (int i = 0; i < buttonSquares.Length; i++)
            {
                buttonSquares[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < buttonSquares.Length; i++)
            {
                if (buttonSquares[i].GetComponentInChildren<TextMeshProUGUI>().text == "")
                {
                    buttonSquares[i].interactable = true;
                }
            }
        }
    }

    public void GetPlayerLetter(string receivedMessage)
    {
        string[] playerLetter = receivedMessage.Split(",");
        Debug.Log(playerLetter);
        player = playerLetter[1];
    }

    public void ProcessOpponentMove(string receivedMessage)
    {
        string[] opponentMove = receivedMessage.Split(",");
        if (opponentMove[1] == "X")
        {
            player = "O";
        }
        Debug.Log(opponentMove[1] + opponentMove[2]);
        int slot = Int32.Parse(opponentMove[2]);
        buttonSquares[slot].GetComponentInChildren<TextMeshProUGUI>().text = opponentMove[1];
        state = GameState.PLAYERTURN;
    }
}
