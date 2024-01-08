using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{

    public Slider backgroundVolumeSlider;
    public Slider sfxVolumeSlider;

    public HomeAudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<HomeAudioManager>();
        backgroundVolumeSlider.value = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void SetBackgroundVolume(float volume)
    {
        audioManager.SetBackgroundVolume(volume);
        PlayerPrefs.SetFloat("BackgroundVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioManager.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
