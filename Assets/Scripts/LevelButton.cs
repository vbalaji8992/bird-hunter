using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
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

        gameObject.transform.Find("LevelScore").GetComponent<Image>().sprite = stars[score];
        gameObject.transform.Find("LevelText").GetComponent<TMPro.TMP_Text>().text = "LEVEL " + levelNumber.ToString("00");

        gameObject.GetComponent<Button>().onClick.AddListener(LoadLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLevel()
    {
        string levelscene = MainMenu.Levels[(int)levelNumber];
        SceneManager.LoadScene(levelscene);
        SceneManager.LoadScene("Player", LoadSceneMode.Additive);
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }
}
