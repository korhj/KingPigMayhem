using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private enum State {
        Playing,
        GameOver,
    }

    private State state;

    private void Start() {
        state = State.Playing;
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) => {state = State.GameOver;};
    }

    private void Update() {
        switch (state) {
            case State.Playing:
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);
    }


}
