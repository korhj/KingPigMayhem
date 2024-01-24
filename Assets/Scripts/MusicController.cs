using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    [SerializeField]
    AudioClip music;

    [SerializeField]
    AudioClip bossMusic;
    private AudioSource audioSource;

    private float volume;
    private bool muted;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("MusicController instance already exists");
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = audioSource.volume;
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

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
