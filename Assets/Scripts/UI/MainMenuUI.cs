using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button quitButton;

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

        MusicController.Instance.PlayMusic();

        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
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
