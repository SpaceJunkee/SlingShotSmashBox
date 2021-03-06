﻿
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour

{

    public Text scoreText;
    public Text scoreMultiplierText;
    public static int score = 0;
    public static int scoreMultiplier = 1;
    public static int scoreMultiplierIncreaser = 1;
    public static int multiplierGoal = 50;

    public static int highScore;

    public static int moneyBagsDrops = 0;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("AbilityTile8") == 1)
        {
            GameObject.FindGameObjectWithTag("BerzerkMeter").GetComponentInChildren<Text>().enabled = true;
            Image[] images = GameObject.FindGameObjectWithTag("BerzerkMeter").GetComponentsInChildren<Image>();
            foreach (Image im in images)
                im.enabled = true;
        }

    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");      
    }

    private void Update()
    {
        scoreText.text = $"{score}";
        scoreMultiplierText.text = $"x{scoreMultiplier}";

        if(score > highScore)
        {
            highScore = score;

            PlayerPrefs.SetInt("HighScore", score);
        }

        if(scoreText.fontSize != 20)
        {
            scoreText.fontSize--;
        }
        else if (scoreText.fontSize == 20)
        {
            scoreText.fontSize = 20;
        }

        if(scoreMultiplierText.fontSize != 15)
        {
            scoreMultiplierText.fontSize--;
        }else if(scoreMultiplierText.fontSize == 15)
        {
            scoreMultiplierText.fontSize = 15;
        }
 
    }

   
}
