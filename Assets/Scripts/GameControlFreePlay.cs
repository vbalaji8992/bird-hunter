using Assets.Scripts;
using System.Linq;
using UnityEngine;

public class GameControlFreePlay : GameControl
{
    public GameObject enemy;
    public GameObject enemyRed;

    protected override void SpawnNewEnemy()
    {
        if (GetAliveEnemies("Hittable") < 4)
            SpawnEnemy(enemy);

        if (GetAliveEnemies("NonHittable") < 1)
            SpawnEnemy(enemyRed);
    }

    private void SpawnEnemy(GameObject enemy)
    {
        var pos1 = GameObject.Find("SpawnPosition-1").transform.position;
        var pos2 = GameObject.Find("SpawnPosition-2").transform.position;
        var spawnPosition = new Vector2(Random.Range(pos1.x, pos2.x), Random.Range(pos1.y, pos2.y));
        Instantiate(enemy, spawnPosition, Quaternion.identity);
    }

    private static int GetAliveEnemies(string type)
    {
        return GameObject.FindGameObjectsWithTag("Enemy")
            .Where(x => x.GetComponent<BirdEnemyFree>().Type == type && x.GetComponent<BirdEnemyFree>().IsDead == false)
            .Count();
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

    public override void DisplayPoint(int point)
    {
        graphicalElement.DisplayPoint(point);
    }
}
