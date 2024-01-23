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

    void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("MusicController instance already exists");
        }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
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
}
