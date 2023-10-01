using System.Collections;
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

    private uint levelNumber;
    private bool isGamePaused;
    protected GraphicalElement graphicalElement;
    protected SaveGame saveGame;
    private int arrowsLeft;
    private uint score;

    public int ArrowsInAir { get; set; }
    public int CurrentEnemies { get; set; }
    public int EnemiesOnGround { get; set; }
    public bool acceptPlayerInput { get; private set; }
    public bool IsLevelOver { get; private set; }
    public float TimeLeft { get; private set; }
    public uint FreePlayKills { get; set; }

    void Awake()
    {
        Time.timeScale = 1f;
        acceptPlayerInput = true;
        IsLevelOver = false;
        TimeLeft = maxTimeInSeconds;

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
        isGamePaused = false;
        graphicalElement = GraphicalElement.Instance;
        saveGame = SaveGame.Instance;

        arrowsLeft = maxArrows;
        graphicalElement.GenerateArrowCount(arrowsLeft);
        graphicalElement.UpdateArrowCountImage(arrowsLeft);
        CurrentEnemies = totalEnemies;
        EnemiesOnGround = 0;
        ArrowsInAir = 0;
        FreePlayKills = 0;

        SetLevelNumberInUI();        
    }

    protected virtual void SetLevelNumberInUI()
    {
        levelNumber = (uint)MainMenu.Levels.IndexOf(SceneManager.GetActiveScene().name);
        graphicalElement.levelNumber.text = "LEVEL " + levelNumber.ToString("00");
        graphicalElement.freePlayKills.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsLevelOver)
            TimeLeft -= Time.deltaTime;

        if (!IsLevelOver && !isGamePaused && arrowsLeft != 0)
            acceptPlayerInput = true;
        else
            acceptPlayerInput = false;

        bool isTimeOver = TimeLeft <= 0;
        bool isNoArrowLeft = arrowsLeft == 0 && ArrowsInAir == 0;
        bool isNoEnemyLeft = CurrentEnemies == 0;
        bool allEnemiesOnGround = EnemiesOnGround == totalEnemies;

        if (!IsLevelOver && (isTimeOver || isNoArrowLeft || isNoEnemyLeft))
            EndLevel();

        if (IsLevelOver && (isTimeOver || isNoArrowLeft || allEnemiesOnGround))
            graphicalElement.DisplayScoreBoard();

        SpawnNewEnemy();
    }

    protected virtual void SpawnNewEnemy()
    {

    }

    public void UpdateArrowCountOnFire()
    {
        arrowsLeft -= 1;
        graphicalElement.UpdateArrowCountImage(arrowsLeft);
        ArrowsInAir += 1;
    }

    private void EndLevel()
    {
        IsLevelOver = true;
        CalculateScore();        
    }

    public void PauseGame()
    {        
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ResumeGame()
    {        
        Time.timeScale = 1f;
        isGamePaused = false;
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

    public void LoadNextLevel()
    {
        if (levelNumber == MainMenu.Levels.Count - 1)
            return;

        string sceneToLoad = MainMenu.Levels[(int)levelNumber + 1];
        var sceneIndexToLoad = SceneUtility.GetBuildIndexByScenePath($"Scenes/{sceneToLoad}");

        if (sceneIndexToLoad >= 0)
        {
            SceneManager.LoadScene(sceneIndexToLoad);
            SceneManager.LoadScene("Player", LoadSceneMode.Additive);
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }

    public virtual void CalculateScore()
    {
        
        float arrowPoints = (float)arrowsLeft / maxArrows;
        float timePoints = TimeLeft / maxTimeInSeconds;

        float totalPoints = (arrowPoints + timePoints) / 2;

        if(CurrentEnemies == 0)
        {
            score = (uint)Math.Ceiling(totalPoints / 0.33);
        }
        else
        {
            score = 0;
        }

        graphicalElement.SetLevelScoreAndStatus(score);
        saveGame.UpdateLevelScore(levelNumber, score);
        saveGame.SaveData();
    }

    public virtual void DisplayPoint(int point)
    {

    }
}
