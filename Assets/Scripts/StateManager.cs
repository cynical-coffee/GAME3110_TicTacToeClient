using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Signifiers
{
    public const int RegisterAccountSignifier = 0;
    public const int LoginAccountSignifier = 1;
    public const int LoggedInSignifier = 2;
    public const int CreateRoomSignifier = 3;
    public const int CreatedRoomSignifier = 4;
    public const int LeaveRoomSignifier = 5;
    public const int StartGameSignifier = 6;
    public const int GamePlaySignifier = 7;
    public const int WinnerSignifier = 8;
    public const int LoserSignifier = 9;
}

public class StateManager : MonoBehaviour
{
    public static StateManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        state = GameState.LOGIN;
    }

    public enum GameState
    {
        CREATEACCOUNT,
        LOGIN,
        CREATEGAMEROOM,
        GAMEROOM
    }

    public GameState state;
}
