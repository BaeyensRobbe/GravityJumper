using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
