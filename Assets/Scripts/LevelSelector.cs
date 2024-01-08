using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public string sceneName;
}

public class LevelSelector : MonoBehaviour
{
    public LevelData[] levels;
    public GameObject[] levelIcons;

    void Start()
    {
        // Setting fill of circles to show progress & set collected coins values
        for (int i = 1; i <= levelIcons.Length;  i++)
        {
            string intName = "Level" + i + "_Progress";
            string collectedCoinsName = "Level" + i + "_CollectedCoins";
            float progress = PlayerPrefs.GetFloat(intName, 0);
            int collectedCoins = PlayerPrefs.GetInt(collectedCoinsName, 0);
            string collectedCoinsText = collectedCoins.ToString() + "/5";
            GameObject childProgress = levelIcons[i - 1].transform.GetChild(2).gameObject;
            GameObject childCoinText = levelIcons[i - 1].transform.GetChild(4).GetChild(0).gameObject;
            childProgress.GetComponent<Image>().fillAmount = progress;
            Debug.Log(progress + "real progress");
            childCoinText.GetComponent<TextMeshProUGUI>().text = collectedCoinsText;
        }




    }

    // Function called when a level button is pressed
    public void OnLevelButtonPressed(int level)
    {
        // Ensure the level number is within the array bounds
        if (level >= 0 && level <= levels.Length)
        {
            // Load the selected level scene
            GameObject homeAudio = GameObject.FindGameObjectWithTag("HomeAudio");
            Destroy(homeAudio);
            SceneManager.LoadScene(levels[level-1].sceneName);
        }
        else
        {
            Debug.LogError("Invalid level number!");
        }
    }
}
