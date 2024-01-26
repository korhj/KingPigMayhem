using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField]
    private AudioClip music;

    [SerializeField]
    private AudioClip bossMusic;
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("MusicController instance already exists");
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("audioSourse == null");
        }
        audioSource.volume = AudioManager.Instance.GetVolume();
    }

    private void Start()
    {
        AudioManager.Instance.OnVolumeChanged += (
            object sender,
            AudioManager.OnVolumeChangedEventArgs e
        ) =>
        {
            audioSource.volume = e.volume;
        };
    }

    public void PlayMusic()
    {
        audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();
    }

    public void PlayBossMusic()
    {
        audioSource.Stop();
        audioSource.clip = bossMusic;
        audioSource.Play();
    }
}
