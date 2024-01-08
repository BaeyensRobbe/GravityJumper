using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinHandler : MonoBehaviour
{
    public List<GameObject> coins;

    public TextMeshProUGUI coinAmountText;

    private int totalCoins = 5;
    // Start is called before the first frame update
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        for (int i = 1; i <= totalCoins; i++)
        {
            string coinName = currentScene + "_coin_" + i;
            Debug.Log(coinName);
            int isCollected = PlayerPrefs.GetInt(coinName);
            if (isCollected == 1)
            {
                coins[i - 1].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
                // Coin is collected, display grey?
            } 
        }

        UpdateCoinAmount();


    }

    public void ResetPlayerPrefs()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        for (int i = 1; i <= totalCoins; i++)
        {
            string coinName = currentScene + "_coin_" + i;
            string progressName = currentScene + "_Progress";
            string collectedCoinsName = currentScene + "_CollectedCoins";
            Debug.Log(coinName);
            Debug.Log(PlayerPrefs.GetFloat(progressName) + "progress");
            PlayerPrefs.SetInt(coinName, 0);
            PlayerPrefs.SetFloat(progressName, 0f);
            PlayerPrefs.SetInt(collectedCoinsName, 0);

        }
    }

    public void UpdateCoinAmount()
    {
        int coinAmount = PlayerPrefs.GetInt("Coins", 0);
        coinAmountText.text = coinAmount.ToString();
    }
}
