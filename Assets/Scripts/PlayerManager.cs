using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class PlayerManager : MonoBehaviour
{
   
    public static bool gameOver;
    public GameObject gameOverPanel;
    public static int numberofCoins;
    public string ishigh;
    public Text coinsText;
    public ScoreManager theScoreManager;
    public Coin Coins;
    public Text finalsc;

    void Start()
    {
        gameOver = false;
        Time.timeScale = 1;
        theScoreManager = FindObjectOfType<ScoreManager>();
        Coins = FindObjectOfType<Coin>();
    }    

    void Update()
    {
        if (theScoreManager.scoreCount>theScoreManager.hiScoreCount)
            {
                ishigh = "NEW HIGH SCORE: ";
            }
        else
            {
                ishigh = "YOUR SCORE IS: ";
            }

        if (gameOver)
        {
            theScoreManager.scoreIncreasing = false;
            Coins.coinsIncreasing = false;
            gameOverPanel.SetActive(true);
            
           
            if (theScoreManager.scoreCount != 0)
            {
                finalsc.text = ishigh+Mathf.Round(theScoreManager.scoreCount + numberofCoins);
                Console.WriteLine(finalsc.text);
            }

            Time.timeScale = 0;
            theScoreManager.scoreCount = 0;
            theScoreManager.scoreIncreasing = true;
            PlayerPrefs.SetInt("0",0);
            PlayerPrefs.Save();
        }
        
        coinsText.text = ""+numberofCoins;
    }
}
