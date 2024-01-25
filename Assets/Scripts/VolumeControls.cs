using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControls : MonoBehaviour
{
    [SerializeField]
    private Button muteButton;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Sprite volumeOnIcon;

    [SerializeField]
    private Sprite volumeOffIcon;

    void Start()
    {
        volumeSlider.value = MusicController.Instance.GetVolume();

        muteButton.onClick.AddListener(() =>
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
            muteButton.image.sprite = volumeOnIcon;
        };
    }

    private void MuteVolume()
    {
        bool muted = MusicController.Instance.Mute();

        if (muted)
        {
            muteButton.image.sprite = volumeOffIcon;
            return;
        }
        muteButton.image.sprite = volumeOnIcon;
    }
}
