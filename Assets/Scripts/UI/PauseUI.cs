using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{

    public event EventHandler OnGamePaused;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;

    private void Start() {
        //Player.Instance.OnPlayerDeath += (object sender, EventArgs e) => {Show();};
        continueButton.onClick.AddListener( ()=> {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Hide();
        });
        mainMenuButton.onClick.AddListener( ()=> {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        pauseButton.onClick.AddListener( () => {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Show();
        });
        Hide();
        
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
