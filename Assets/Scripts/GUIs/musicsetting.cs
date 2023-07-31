using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class musicsetting : MonoBehaviour
{
    [SerializeField] private AudioMixer musicSource;
    [SerializeField] private Slider musicSlider;
   

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            Loadvolume();
        }
        else
        {
            SetMusicVolume();
        }
       
    }
    void start()
    {
        
    }
  

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicSource.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void Loadvolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SetMusicVolume();
    }
    public void Onmusicbuttonclick()
    {
      
    }
    public void Offmusic()
    {
        
    }
}
