using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audiomanager : MonoBehaviour
{
    public static audiomanager instance;
    public sound[] musicSounds, sfxSounds;
    public AudioSource musicSource,sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("theme");
    }
    public void PlayMusic(string name)
    {
        sound s =Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("sound not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
       
    }
    public void PlaySFX(string name)
    {
        sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("sound not Found");
        }
        else
        {
            sfxSource.clip = s.clip;
            
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
