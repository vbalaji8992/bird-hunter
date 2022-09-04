using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighestScore : MonoBehaviour
{
    public uint levelNumber;

    public Sprite[] stars;

    private SaveGame saveGame;
    private uint score;

    // Start is called before the first frame update
    void Start()
    {
        saveGame = SaveGame.Instance;

        score = saveGame.GetScoreForLevel(levelNumber);
        Debug.Log(score);
        gameObject.GetComponent<Image>().sprite = stars[score];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
