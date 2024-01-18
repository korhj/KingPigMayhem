using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField]
    private Button playAgainButton;

    [SerializeField]
    private Button mainMenuButton;

    [SerializeField]
    private TextMeshProUGUI finalScoreText;

    [SerializeField]
    ExitDoor exitDoor;

    private void Start()
    {
        exitDoor.OnEnter += (object sender, EventArgs e) =>
        {
            Debug.Log("VictoryUI exitDoor.OnEnter");
            Show();
        };
        playAgainButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        finalScoreText.text = "Score: " + Player.Instance.GetScore().ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
