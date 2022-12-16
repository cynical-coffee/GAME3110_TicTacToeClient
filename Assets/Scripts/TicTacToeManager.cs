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
    private int[] gameBoard;
    [SerializeField] private GameObject buttonGrid;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;

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
        gameBoard = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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
            case GameState.WIN:
                isPlayerTurn = false;
                break;
            case GameState.LOSE:
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
        gameBoard[buttonNum] = 1;
        CheckWinConditions();
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

    private void CheckWinConditions()
    {
        int row1 = gameBoard[0] + gameBoard[1] + gameBoard[2];
        int row2 = gameBoard[3] + gameBoard[4] + gameBoard[5];
        int row3 = gameBoard[6] + gameBoard[7] + gameBoard[8];

        int col1 = gameBoard[0] + gameBoard[3] + gameBoard[6];
        int col2 = gameBoard[1] + gameBoard[4] + gameBoard[7];
        int col3 = gameBoard[2] + gameBoard[5] + gameBoard[8];

        int cross1 = gameBoard[0] + gameBoard[4] + gameBoard[8];
        int cross2 = gameBoard[2] + gameBoard[4] + gameBoard[6];

        Debug.Log($"row 1: {row1}");
        Debug.Log($"row 2: {row2}");
        Debug.Log($"row 3: {row3}");

        Debug.Log($"col 1: {col1}");
        Debug.Log($"col 2: {col2}");
        Debug.Log($"col 3: {col3}");

        Debug.Log($"cross 1: {col3}");
        Debug.Log($"cross 2: {col3}");

        int[] solutions = new int[] { row1, row2, row3, col1, col2, col3, cross1, cross2 };
        for (int i = 0; i < solutions.Length; i++)
        {
            if (solutions[i] == 3)
            {
                Debug.Log("Player Wins!" + solutions[i] + player);
                client.SendMessageToHost(Signifiers.WinnerSignifier.ToString());
            }
        }
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

    public void WinGame()
    {
        state = GameState.WIN;
        winText.gameObject.SetActive(true);
    }

    public void LoseGame()
    {
        state = GameState.LOSE;
        loseText.gameObject.SetActive(true);
    }

    public void GetPlayerLetter(string receivedMessage)
    {
        string[] playerLetter = receivedMessage.Split(",");
        Debug.Log(playerLetter);
        player = playerLetter[1];
    }
}
