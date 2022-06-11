using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] spaceshipSounds;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int sound)
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(spaceshipSounds[sound]);
    }
}
