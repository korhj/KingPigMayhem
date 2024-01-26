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
        volumeSlider.value = AudioManager.Instance.GetVolume();

        muteButton.onClick.AddListener(() =>
        {
            MuteVolume();
        });

        volumeSlider.onValueChanged.AddListener(
            (sliderValue) =>
            {
                AudioManager.Instance.SetVolume(sliderValue);
            }
        );

        AudioManager.Instance.OnVolumeChanged += (
            object sender,
            AudioManager.OnVolumeChangedEventArgs e
        ) =>
        {
            muteButton.image.sprite = volumeOnIcon;
        };
    }

    private void MuteVolume()
    {
        bool muted = AudioManager.Instance.Mute();

        if (muted)
        {
            muteButton.image.sprite = volumeOffIcon;
            return;
        }
        muteButton.image.sprite = volumeOnIcon;
    }
}
