using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private const string PLAYERPREFS_VOLUME = "PlayerPrefsVolume";

    public static MusicController Instance { get; private set; }
    public event EventHandler OnVolumeChanged;

    [SerializeField]
    AudioClip music;

    [SerializeField]
    AudioClip bossMusic;
    private AudioSource audioSource;

    static float volume;
    private bool muted;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("MusicController instance already exists");
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYERPREFS_VOLUME, 0.5f);
        audioSource.volume = volume;
    }

    public void PlayMusic()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        if (audioSource == null)
            return;
        audioSource.Stop();
        audioSource.clip = bossMusic;
        audioSource.Play();
    }

    public bool Mute()
    {
        if (muted)
        {
            audioSource.volume = volume;
            muted = false;
            return muted;
        }

        muted = true;
        volume = audioSource.volume;
        audioSource.volume = 0;
        return muted;
    }

    public void SetVolume(float sliderValue)
    {
        OnVolumeChanged?.Invoke(this, EventArgs.Empty);
        audioSource.volume = sliderValue;

        PlayerPrefs.SetFloat(PLAYERPREFS_VOLUME, sliderValue);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }
}
