using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public event EventHandler OnGamePaused;

    [SerializeField]
    private Button pauseButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button mainMenuButton;

    [SerializeField]
    private Button volumeButton;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Sprite volumeOnIcon;

    [SerializeField]
    private Sprite volumeOffIcon;

    private void Start()
    {
        volumeSlider.value = MusicController.Instance.GetVolume();

        continueButton.onClick.AddListener(() =>
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Hide();
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        pauseButton.onClick.AddListener(() =>
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Show();
        });

        volumeButton.onClick.AddListener(() =>
        {
            MuteVolume();
        });

        volumeSlider.onValueChanged.AddListener(
            (sliderValue) =>
            {
                MusicController.Instance.SetVolume(sliderValue);
            }
        );

        MusicController.Instance.OnVolumeChanged += (object sender, EventArgs e) =>
        {
            volumeButton.image.sprite = volumeOnIcon;
        };

        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void MuteVolume()
    {
        bool muted = MusicController.Instance.Mute();

        if (muted)
        {
            volumeButton.image.sprite = volumeOffIcon;
            return;
        }
        volumeButton.image.sprite = volumeOnIcon;
    }
}
