using System.Text.RegularExpressions;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct BackGroundMusic
    {
        public int level;
        public AudioClip music;
    }

    [System.Serializable]
    public struct SFX
    {
        public string name;
        public AudioClip soundEffect;
    }


    private AudioSource audioSource;
    private AudioSource sfxAudioSource;

    public BackGroundMusic[] backGroundMusicList;
    public SFX[] soundEffectsList;

    private bool sfxIsPlaying = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sfxAudioSource = gameObject.AddComponent<AudioSource>();

        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLevel = ExtractNumberFromSceneName(currentSceneName);

        foreach (BackGroundMusic bgm in backGroundMusicList)
        {
            if (bgm.level == currentLevel)
            {
                audioSource.clip = bgm.music;
                audioSource.Play();
            }
        }
    }

    public void PlaySoundEffect(string name)
    {
        foreach (SFX sfx in soundEffectsList)
        {
            if (sfx.name == name)
            {
                if (sfx.name == "PlayerDies")
                {
                    sfxAudioSource.clip = sfx.soundEffect;
                    sfxAudioSource.Play();
                    sfxIsPlaying = true;
                } else
                {
                    sfxAudioSource.clip = sfx.soundEffect;
                    sfxAudioSource.Play();
                }
            }
        }
    }

    public void StopPlayingBackgroundMusic()
    {
        audioSource.Stop();
    }

    private int ExtractNumberFromSceneName(string sceneName)
    {
        // Use regular expression to match the number in the scene name
        Match match = Regex.Match(sceneName, @"\d+");

        // If a match is found, convert it to an integer
        if (match.Success)
        {
            return Convert.ToInt32(match.Value);
        }

        // Return -1 if no match is found
        return -1;
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    public void SetBackgroundVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }



}
