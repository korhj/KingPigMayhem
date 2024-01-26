using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const string PLAYERPREFS_VOLUME = "PlayerPrefsVolume";

    public static AudioManager Instance { get; private set; }
    public event EventHandler<OnVolumeChangedEventArgs> OnVolumeChanged;

    [SerializeField]
    private AudioSource soundEffectObject;

    [SerializeField]
    private float soundEffectVolume;

    public class OnVolumeChangedEventArgs
    {
        public float volume;
    }

    private bool muted;
    private float audioVolume;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("MusicController instance already exists");
        }
        Instance = this;

        muted = false;
        audioVolume = PlayerPrefs.GetFloat(PLAYERPREFS_VOLUME, 0.5f);
    }

    public void SetVolume(float sliderValue)
    {
        OnVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { volume = sliderValue });

        PlayerPrefs.SetFloat(PLAYERPREFS_VOLUME, sliderValue);
        PlayerPrefs.Save();
        audioVolume = sliderValue;
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat(PLAYERPREFS_VOLUME, 0.5f);
    }

    public bool Mute()
    {
        muted = !muted;

        if (muted)
        {
            OnVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { volume = 0 });
            return muted;
        }
        OnVolumeChanged?.Invoke(this, new OnVolumeChangedEventArgs { volume = audioVolume });
        return muted;
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(
            soundEffectObject,
            Camera.main.transform.position,
            Quaternion.identity
        );

        audioSource.clip = audioClip;
        audioSource.volume = audioVolume * soundEffectVolume;
        audioSource.Play();

        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
