using Assets.Scripts;
using System.Linq;
using UnityEngine;

public class GameControlFreePlay : GameControl
{
    public GameObject enemy;

    protected override void SpawnNewEnemy()
    {
        int enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy")
            .Where(x => x.GetComponent<BirdEnemyFree>().IsDead == false)
            .Count();

        if (enemiesAlive < 4)
        {
            var pos1 = GameObject.Find("SpawnPosition-1").transform.position;
            var pos2 = GameObject.Find("SpawnPosition-2").transform.position;
            var spawnPosition = new Vector2(Random.Range(pos1.x, pos2.x), Random.Range(pos1.y, pos2.y));
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }

    public override void CalculateScore()
    {
        graphicalElement.freePlayKills.text = FreePlayKills.ToString();
        saveGame.UpdateArcadeScore(FreePlayKills);
        saveGame.SaveData();

        if(isLevelOver)
            graphicalElement.SetArcadeScore(FreePlayKills);
    }

    protected override void SetLevelNumberInUI()
    {
        graphicalElement.levelNumber.text = "ARCADE";        
        CalculateScore();
    }
}
