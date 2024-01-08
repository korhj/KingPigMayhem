using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button tryAgainButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI finalScoreText;

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
        finalScoreText.text = "Score: " + Player.Instance.GetScore().ToString();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
