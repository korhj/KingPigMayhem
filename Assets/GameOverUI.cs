using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start() {
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) => {Show();};
        tryAgainButton.onClick.AddListener( ()=> {
            Loader.Load(Loader.Scene.GameScene);
        });
        mainMenuButton.onClick.AddListener( ()=> {
            Loader.Load(Loader.Scene.MainMenuScene);
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
