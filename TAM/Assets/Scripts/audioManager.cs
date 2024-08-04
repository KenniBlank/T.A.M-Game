using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [Header("---- Audio Source ----")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---- Audio Clip Player ----")]
    public AudioClip respawnSound;
    public AudioClip dashSound;
    public AudioClip jumpSound;
    public AudioClip landingSound;

    // SoundEffectEnemy
    [Header("---- For All ----")]
    public AudioClip backgroundMusic; // Music
    public AudioClip walk;
    public AudioClip attack;
    public AudioClip dead;
    public AudioClip magic;

    // Singleton instance
    public static audioManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Makes sure the instance is persistent across scenes
        }
    }

    private void Start()
    {
        musicSource.volume = 0.5f;
        SFXSource.volume = 0.5f;
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // Method to change the music volume
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    // Method to change the SFX volume
    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}