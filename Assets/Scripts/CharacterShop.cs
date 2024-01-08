using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterShop : MonoBehaviour
{
    [System.Serializable]
    public struct Colors
    {
        public Color color;
        public string name;
    }

    
    public Colors[] colors;
    public Sprite[] sprites;

    public GameObject sprite;

    public TextMeshProUGUI errorMessage;

    private Image characterColor;
    private Image characterSprite;

    private string colorInShop;
    private int spriteInShop;



    // Start is called before the first frame update
    void Start()
    {
        // initialize color and sprite objects
        characterColor = sprite.transform.GetChild(0).gameObject.GetComponent<Image>();
        characterSprite = sprite.transform.GetChild(1).gameObject.GetComponent<Image>();

        // get current color and sprite
        colorInShop = PlayerPrefs.GetString("CurrentColor", "Red");
        spriteInShop = PlayerPrefs.GetInt("CurrentSprite", 0);


        // asign color and sprite
        characterColor.color = colors.FirstOrDefault(color => color.name == colorInShop).color;
        characterSprite.sprite = sprites[spriteInShop];

        HandleColorLocks();
        
    }

    private void CanvasHandler()
    {
        throw new NotImplementedException();
    }

    public void ChangeSpriteInShop(string action)
    {
        if (action == "Next")
        {
            spriteInShop++;
            if (spriteInShop >= sprites.Length)
            {
                spriteInShop = 0;
            }
        } else
        {
            spriteInShop--;
            if (spriteInShop < 0)
            {
                spriteInShop = sprites.Length - 1;
            }
        }

        characterSprite.sprite = sprites[spriteInShop];
        
    }

    public void SelectColor(string chosenColor)
    {
        Color selectedColor = colors.FirstOrDefault(color => color.name == chosenColor).color;
        if (selectedColor != null)
        {
            if (!GameObject.Find(chosenColor).transform.GetChild(2).gameObject.activeSelf)
            {
                colorInShop = chosenColor;
                characterColor.color = selectedColor;
                errorMessage.gameObject.SetActive(false);
            } else
            {
                errorMessage.gameObject.SetActive(true);
                errorMessage.text = DisplayCorrectText(chosenColor);
                // show why it is locked
            }

        }
    }

    public void Play()
    {
        PlayerPrefs.SetInt("CurrentSprite", spriteInShop);
        PlayerPrefs.SetString("CurrentColor", colorInShop);
        SceneManager.LoadScene("LevelSelector");
    }

    private void HandleColorLocks()
    {
        GameObject.Find("Red").transform.GetChild(2).gameObject.SetActive(false);

        int level1 = PlayerPrefs.GetInt("Level1", 0);
        int level2 = PlayerPrefs.GetInt("Level2", 0);
        int level3 = PlayerPrefs.GetInt("Level3", 0);

        if(level1 == 1)
        {
            string[] colorsToSetLocksFalse = { "Blue", "Green" };
            SetLocksFalse(colorsToSetLocksFalse);
        }
        if (level2 == 1)
        {
            string[] colorsToSetLocksFalse = { "Pink", "Yellow" };
            SetLocksFalse(colorsToSetLocksFalse);
        }
        if (level3 == 1)
        {
            string[] colorsToSetLocksFalse = { "Brown", "Orange" };
            SetLocksFalse(colorsToSetLocksFalse);
        }

        if (PlayerPrefs.GetInt("Coins") == 15)
        {
            string[] lightBlue = { "LightBlue" };
            SetLocksFalse(lightBlue);
        }


    }

    private void SetLocksFalse(string[] colors)
    {
        foreach (string color in colors)
        {
            GameObject g = GameObject.Find(color);
            g.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private string DisplayCorrectText(string chosenColor)
    {
        int level = 0;
        switch (chosenColor)
        {
            case "Blue":
            case "Green":
                level = 1;
                break;
            case "Pink":
            case "Yellow":
                level = 2;
                break;
            case "Brown":
            case "Orange":
                level = 3;
                break;
            case "LightBlue":
                level = 4;
                break;
            default:
                return "";
                break;
        }

        if (level == 4)
        {
            return "COLLECT ALL 15 COINS TO UNLOCK THIS COLOR";
        }
        else
        {
            return $"COMPLETE LEVEL {level} TO UNLOCK THIS COLOR";
        }
    }
}
