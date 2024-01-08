using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPopupHandler : MonoBehaviour
{
    public TextMeshProUGUI completionText;
    public TextMeshProUGUI collectedCoinsText;
    public TextMeshProUGUI bestCompletionText;
    public TextMeshProUGUI levelText;
    // Start is called before the first frame update
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string savedCompletionString = currentScene + "_LastTry";
        string progress = currentScene + "_Progress";
        float completion = Mathf.Round((PlayerPrefs.GetFloat(savedCompletionString) * 100f));
        string completionString = "COMPLETION: \n" + completion + "%";
        int collectedCoins = PlayerPrefs.GetInt(currentScene + "_CollectedCoins");
        string collectedCoinsString = collectedCoins.ToString() + "/5";
        float bestCompletion = Mathf.Round((PlayerPrefs.GetFloat(progress) * 100f));
        if (completion == 100f)
        {
            levelText.text = "LEVEL " + currentScene[5] + " COMPLETED";
            levelText.color = Color.green;
        } else if (completion == bestCompletion)
        {
            levelText.text = "NEW HIGHSCORE!";
            levelText.color = Color.green;
        }
        else
        {
            levelText.text = "TRY AGAIN";
            levelText.color = Color.red;
        }
        bestCompletionText.text = "BEST:\n" + bestCompletion + "%";
        completionText.text = completionString;
        collectedCoinsText.text = collectedCoinsString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
