using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffectsController : MonoBehaviour
{
    public static SoundEffectsController Instance { get; private set; }

    [SerializeField]
    AudioClip fireballSound;

    [SerializeField]
    AudioClip takeDamageSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("audioSourse == null");
        }
    }

    public void PlayTakeDamageSound()
    {
        audioSource.Play();
    }
}
