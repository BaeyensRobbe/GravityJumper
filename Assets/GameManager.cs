using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public GameObject settingsPopup;

    public GameObject panel;
    public GameObject pauseMenu;

    PlayerController playerController;
    AudioManager audioManager;
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelSelector()
    {
        SceneManager.LoadScene("LevelSelector");
    }

    public void ChangeCharacter()
    {
        SceneManager.LoadScene("CharacterShop");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SettingsPopup()
    {
        settingsPopup.SetActive(true);
    }

    public void CloseSettingsPopup()
    {
        settingsPopup.SetActive(false);
    }

    public void PauseGame()
    {
        playerController.ToggleCanJump();
        panel.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        audioManager.PauseMusic();
    }

    public void ResumeGame()
    {
        playerController.ToggleCanJump();
        panel.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        audioManager.ResumeMusic();
    }
}
