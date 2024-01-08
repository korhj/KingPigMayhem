using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }

    [SerializeField] PauseUI pauseUI;
    [SerializeField] float scoreIncrease = 10; 

    public event EventHandler OnStateChange;

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

    private void Start() {
        timeScale =Time.timeScale;
        countdownTimer = 0f;
        scoreTimer = 0f;
        gamePaused = false;
        state = State.Countdown;
        previousState = state;
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) => {state = State.GameOver; OnStateChange?.Invoke(this, EventArgs.Empty);};
        pauseUI.OnGamePaused += GameManager_OnGamePaused;
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
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        switch (state) {
            case State.Countdown:
                if(countdownTimer >= 3) {
                    state = State.Playing;
                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                countdownTimer += Time.deltaTime;
                break;
            case State.Playing:
                if(scoreTimer >= 10){
                    Player.Instance.IncreaseScore(scoreIncrease);
                    scoreTimer = 0;
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


}
