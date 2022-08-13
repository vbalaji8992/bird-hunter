﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using TMPro;
using Assets.Scripts;

public class GameControl : MonoBehaviour
{

    public static GameControl Instance { get; private set; }

    public int maxArrows;
    public int totalEnemies;
    public float maxTimeInSeconds;

    private GraphicalElement graphicalElement;

    [HideInInspector]
    public int arrowsLeft;

    [HideInInspector]
    public int arrowsInAir;

    [HideInInspector]
    public int currentEnemies;

    private int score;

    [HideInInspector]
    public bool acceptPlayerInput;

    [HideInInspector]
    public bool isLevelOver;

    [HideInInspector]
    public float timeLeft;

    void Awake()
    {
        Time.timeScale = 1f;
        acceptPlayerInput = true;
        isLevelOver = false;
        timeLeft = maxTimeInSeconds;

        if (Instance == null)
        {
            Instance = this;
        }            

        else if (Instance != this)
        {
            Destroy(gameObject);
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        graphicalElement = GraphicalElement.Instance;

        arrowsLeft = maxArrows;
        graphicalElement.UpdateArrowCountImage(arrowsLeft);
        currentEnemies = totalEnemies;
        arrowsInAir = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLevelOver)
        {
            timeLeft -= Time.deltaTime;
        }

        bool isTimeOver = timeLeft <= 0;
        bool isNoArrowLeft = arrowsLeft == 0 && arrowsInAir == 0;
        bool isNoEnemyLeft = currentEnemies == 0;

        if (!isLevelOver && (isTimeOver || isNoArrowLeft || isNoEnemyLeft))
        {
            EndLevel();
        }
    }

    public void UpdateArrowCountOnFire()
    {
        arrowsLeft -= 1;
        graphicalElement.UpdateArrowCountImage(arrowsLeft);
        arrowsInAir += 1;
    }

    private void EndLevel()
    {
        isLevelOver = true;
        acceptPlayerInput = false;
        CalculateScore();
        graphicalElement.DisplayScoreBoard();
    }

    public void PauseGame()
    {        
        Time.timeScale = 0f;
        acceptPlayerInput = false;
    }

    public void ResumeGame()
    {        
        Time.timeScale = 1f;
        acceptPlayerInput = true;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("Player", LoadSceneMode.Additive);
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    

    void CalculateScore()
    {
        
        float arrowPoints = (float)arrowsLeft / maxArrows;
        float timePoints = timeLeft / maxTimeInSeconds;

        float totalPoints = (arrowPoints + timePoints) / 2;

        if(currentEnemies == 0)
        {
            score = (int)Math.Ceiling(totalPoints / 0.33);
        }
        else
        {
            score = 0;
        }

       graphicalElement.SetLevelScoreAndStatus(score);
        
    }
}