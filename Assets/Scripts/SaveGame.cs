using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class SaveGame : MonoBehaviour
{
    public static SaveGame Instance { get; private set; }

    public SaveData CurrentData { get; private set; }

    private string saveFileNameWithPath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        saveFileNameWithPath = Application.persistentDataPath + "/SaveData.json";

        CurrentData = LoadData();
        SaveData();

        DontDestroyOnLoad(gameObject);
    }

    public void SaveData()
    {        
        string saveData = JsonUtility.ToJson(CurrentData);

        File.WriteAllText(saveFileNameWithPath, saveData);
    }

    private SaveData LoadData()
    {
        if (File.Exists(saveFileNameWithPath))
        {
            string saveData = File.ReadAllText(saveFileNameWithPath);
            var data = JsonUtility.FromJson<SaveData>(saveData);
            return ValidateData(data);
        }
        else
        {
            return new SaveData(true, 0);
        }
    }

    private SaveData ValidateData(SaveData data)
    {
        SaveData validatedData = new SaveData(data.showHand, data.highestScoreArcade);

        foreach (var rating in data.levelScores)
        {
            if (rating.highestScore > 3)
            {
                validatedData.levelScores.Add(new LevelScore(rating.levelNumber, 0));
            }
            else
            {
                validatedData.levelScores.Add(rating);
            }
        }

        return validatedData;
    }

    public void UpdateLevelScore(uint levelNumber, uint score)
    {
        var levelScore = CurrentData.levelScores.FirstOrDefault(item => item.levelNumber == levelNumber);

        if (levelScore != null)
        {
            if (levelScore.highestScore > score)
                return;

            CurrentData.levelScores.Find(item => item.levelNumber == levelNumber).highestScore = score;
        }
        else
        {
            CurrentData.levelScores.Add(new LevelScore(levelNumber, score));
        }
    }

    public uint GetScoreForLevel(uint levelNumber)
    {
        var levelScore = CurrentData.levelScores.FirstOrDefault(item => item.levelNumber == levelNumber);

        if (levelScore != null)
        {
            return levelScore.highestScore;
        }
        else
        {
            return 0;
        }
    }

    public uint GetArcadeScore()
    {
        return CurrentData.highestScoreArcade;
    }

    public void UpdateArcadeScore(uint score)
    {
        if (score > CurrentData.highestScoreArcade)
            CurrentData.highestScoreArcade = score;
    }
}

[Serializable]
public class SaveData
{
    public bool showHand;
    public List<LevelScore> levelScores;
    public uint highestScoreArcade;

    public SaveData(bool _showHand, uint _highestScoreArcade)
    {
        showHand = _showHand;
        levelScores = new List<LevelScore>();
        highestScoreArcade = _highestScoreArcade;
    }
}

[Serializable]
public class LevelScore
{
    public uint levelNumber;
    public uint highestScore;

    public LevelScore(uint _levelNumber, uint _highestRating)
    {
        levelNumber = _levelNumber;
        highestScore = _highestRating;
    }    
}
