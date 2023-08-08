using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreArcade;

    public static List<string> Levels { get; private set; } = new List<string>
        {
            "BirdStaticNear",
            "BirdStaticFar",
            "BirdMoving",
            "BirdBehindPlank",            
            "BirdUpDown",
            "BirdUpDownPlank",
            "BirdTwoMoving",
            "BirdBetweenPlanks",
            "BirdPlankAndMetal",
            "BirdTwoMetals"
        };

    // Start is called before the first frame update
    void Start()
    {        
        highScoreArcade.text = "HIGH SCORE - " + SaveGame.Instance.GetArcadeScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayArcade()
    {
        LevelButton.LoadLevelAdditive("FreePlay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
