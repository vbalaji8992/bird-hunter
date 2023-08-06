using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
            "BirdTwoMetals",
            "FreePlay"
        };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
