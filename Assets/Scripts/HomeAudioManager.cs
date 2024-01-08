using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeAudioManager : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        GameObject[] musicObject = GameObject.FindGameObjectsWithTag("HomeAudio");
        if (musicObject.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
