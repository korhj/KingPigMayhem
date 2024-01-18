using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    PauseUI pauseUI;

    [SerializeField]
    Room room;

    [SerializeField]
    Transform key;

    [SerializeField]
    ExitDoor exitDoor;

    //[SerializeField] float scoreIncrease = 10;

    public event EventHandler OnStartPlaying;

    private enum State
    {
        Countdown,
        Playing,
        Paused,
        GameOver,
    }

    private State state;
    private float countdownTimer = 0f;
    private float scoreTimer = 0f;
    private bool gamePaused;
    private float timeScale;
    private State previousState;
    private int[,] mapArray;
    private Room[,] roomArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("GameManager instance already exists");
        }
        Instance = this;
        mapArray = new int[,]
        {
            { 1, 1, 1, 1, 0 },
            { 1, 0, 1, 0, 0 },
            { 0, 0, 1, 1, 1 },
            { 0, 0, 1, 1, 1 },
            { 0, 0, 1, 0, 0 }
        };

        roomArray = new Room[mapArray.GetLength(0), mapArray.GetLength(1)];
    }

    private void Start()
    {
        timeScale = Time.timeScale;
        countdownTimer = 0f;
        scoreTimer = 0f;
        gamePaused = false;
        state = State.Countdown;
        previousState = state;
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) =>
        {
            state = State.GameOver;
        };
        pauseUI.OnGamePaused += GameManager_OnGamePaused;
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) =>
        {
            state = State.GameOver;
        };
        exitDoor.OnEnter += (object sender, EventArgs e) =>
        {
            Debug.Log("GameManager exitDoor.OnEnter");
            state = State.GameOver;
        };

        SpawnRooms();
        SpawnKey();
        SpawnExitDoor();
    }

    private void SpawnKey()
    {
        static List<(int, int)> FindRooms(int[,] matrix)
        {
            var cells = new List<(int, int)>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        cells.Add((i, j));
                    }
                }
            }
            return cells;
        }

        var roomLocations = FindRooms(mapArray);

        System.Random random = new System.Random();
        int randomIndex = random.Next(roomLocations.Count);
        (int row, int col) = roomLocations[randomIndex];

        Instantiate(key, new Vector3(col * 35, 0, -row * 20), Quaternion.identity);
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e)
    {
        gamePaused = !gamePaused;
        if (gamePaused)
        {
            previousState = state;
            state = State.Paused;
            return;
        }
        state = previousState;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Countdown:
                if (countdownTimer >= 1)
                {
                    state = State.Playing;
                    OnStartPlaying?.Invoke(this, EventArgs.Empty);
                }
                countdownTimer += Time.deltaTime;
                break;
            case State.Playing:
                Time.timeScale = timeScale;
                if (scoreTimer >= 10)
                {
                    //Player.Instance.IncreaseScore(scoreIncrease);
                    scoreTimer = 0;
                    roomArray[0, 0].playerEntered();
                }
                scoreTimer += Time.deltaTime;
                break;
            case State.Paused:
                Time.timeScale = 0;
                break;
            case State.GameOver:
                Time.timeScale = 0;
                break;
        }
        //Debug.Log(state);
    }

    private void SpawnRooms()
    {
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (mapArray[i, j] != 0)
                {
                    roomArray[i, j] = Instantiate(
                        room,
                        new Vector3(j * 35, 0, -i * 20),
                        Quaternion.identity
                    );
                }
                else
                {
                    roomArray[i, j] = null;
                }
            }
        }
    }

    private void SpawnExitDoor()
    {
        Instantiate(
            exitDoor.transform,
            roomArray[3, 4].transform.position - new Vector3(0, 0, 7),
            Quaternion.identity
        );
        roomArray[3, 4].SpawnExit(exitDoor);
    }

    public Room[] GetAdjacedRooms(Vector3 roomPosition)
    {
        Room[] roomInDirection = new Room[4] { null, null, null, null };
        int j = (int)roomPosition.x / 35;
        int i = -(int)roomPosition.z / 20;
        if (i - 1 >= 0)
        {
            if (roomArray[i - 1, j] != null)
            {
                roomInDirection[0] = roomArray[i - 1, j];
            }
        }
        if (i + 1 <= mapArray.GetLength(0) - 1)
        {
            if (roomArray[i + 1, j] != null)
            {
                roomInDirection[1] = roomArray[i + 1, j];
            }
        }
        if (j - 1 >= 0)
        {
            if (roomArray[i, j - 1] != null)
            {
                roomInDirection[2] = roomArray[i, j - 1];
            }
        }
        if (j + 1 <= mapArray.GetLength(1) - 1)
        {
            if (roomArray[i, j + 1] != null)
            {
                roomInDirection[3] = roomArray[i, j + 1];
            }
        }

        return roomInDirection;
    }
}
