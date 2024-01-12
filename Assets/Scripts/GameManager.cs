using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [SerializeField] PauseUI pauseUI;
    [SerializeField] Room room;
    //[SerializeField] float scoreIncrease = 10; 

    public event EventHandler OnStartPlaying;

    private enum State {
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

    private void Awake() {
        if (Instance != null){
            Debug.Log("GameManager instance already exists");
        }
        Instance = this;
        mapArray = new int[,] { {1,1,1,1,0},
                                {1,0,1,0,0},
                                {0,0,1,1,1},
                                {0,0,1,1,2},
                                {0,0,3,0,0}};

        roomArray = new Room[mapArray.GetLength(0),mapArray.GetLength(1)];

    }

    private void Start() {
        timeScale =Time.timeScale;
        countdownTimer = 0f;
        scoreTimer = 0f;
        gamePaused = false;
        state = State.Countdown;
        previousState = state;
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) => {state = State.GameOver;};
        pauseUI.OnGamePaused += GameManager_OnGamePaused;
        SpawnRooms();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e) {
        gamePaused = !gamePaused;
        if (gamePaused) {
            previousState = state;
            state = State.Paused;
        }
        else {
            state = previousState;
        }
        if (Time.timeScale != 0) {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = timeScale;
        }
    }

    private void Update() {
        switch (state) {
            case State.Countdown:
                if(countdownTimer >= 3) {
                    state = State.Playing;
                    OnStartPlaying?.Invoke(this, EventArgs.Empty);
                }
                countdownTimer += Time.deltaTime;
                break;
            case State.Playing:
                if(scoreTimer >= 10){
                    //Player.Instance.IncreaseScore(scoreIncrease);
                    scoreTimer = 0;
                    roomArray[0,0].playerEntered();
                }
                scoreTimer += Time.deltaTime;
                break;
            case State.Paused:
                break;
            case State.GameOver:
                break;
        }
        //Debug.Log(state);
    }

    private void SpawnRooms() {
        for(int i = 0; i < mapArray.GetLength(0); i++) {
            for(int j = 0; j < mapArray.GetLength(1); j++) {
                if(mapArray[i,j] != 0) {
                    roomArray[i,j] = Instantiate(room, new Vector3(j*35, 0, -i*20), Quaternion.identity);

                }
                else{
                    roomArray[i,j] = null;
                }
            }
        }
    }
    public Room[] GetAdjacedRooms(Vector3 roomPosition) {
        Room[] roomInDirection = new Room[4] {null, null, null, null};
        int j = (int)roomPosition.x / 35;
        int i = -(int)roomPosition.z / 20;
        if(i-1 >= 0) {
            if(roomArray[i-1,j] != null) {
                roomInDirection[0] = roomArray[i-1,j]; 
            }
        }
        if(i+1 <= mapArray.GetLength(0) - 1) {
            if(roomArray[i+1,j] != null) {
                roomInDirection[1] = roomArray[i+1,j]; 
            }
        }
        if(j-1 >= 0) {
            if(roomArray[i,j-1] != null) {
                roomInDirection[2] = roomArray[i,j-1]; 
            }
        }
        if(j+1 <= mapArray.GetLength(1) - 1) {
            if(roomArray[i,j+1] != null) {
                roomInDirection[3] = roomArray[i,j+1]; 
            }
        }
        
        return roomInDirection;

    }

}
