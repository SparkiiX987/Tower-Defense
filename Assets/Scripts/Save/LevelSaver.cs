using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSaver : MonoBehaviour
{
    [Header("LevelData")]
    [SerializeField] private int level;
    [SerializeField] private PlayerGolds playerGolds;
    [SerializeField] private PlayerHealthManager playerHealth;
    [SerializeField] private List<TowerBehaviour> towers = new List<TowerBehaviour>();
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    [SerializeField] private int levelsNumber;

    [Header("UI")]
    [SerializeField] private GameObject loadUI;
    [SerializeField] private GameObject startNextWaveButton;
    [SerializeField] private GameObject tutoPanel;
    [SerializeField] private GameObject playerController;

    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private string saveFilePath;

    private void Start()
    {
        if (level == 0) { return; }
        saveFilePath = Application.persistentDataPath + "/Level" + level + ".json";
        startNextWaveButton.SetActive(false);
        playerController.SetActive(false);
        tutoPanel.SetActive(false);
        if (!File.Exists(saveFilePath))
        {
            loadUI.SetActive(false);
            if(tutoPanel.GetComponent<Tuto>().textsCount == 0)
            {
                startNextWaveButton.SetActive(true);
                playerController.SetActive(true);
            }
            tutoPanel.SetActive(true);
            return;
        }
    }

    public void DeleteAllFiles()
    {
        for (int i = 1; i < levelsNumber + 1; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/Level" + i + ".json"))
            {
                File.Delete(Application.persistentDataPath + "/Level" + i + ".json");
            }
        }
    }

    public void SaveActualLevel()
    {
        LevelSaveData levelSave = new LevelSaveData();

        levelSave.health = playerHealth.GetHealth();
        levelSave.gold = playerGolds.GetGold();

        foreach (TowerBehaviour tower in towers)
        {
            levelSave.towers.Add(tower.GetTowerData());
        }

        levelSave.currentWave = enemiesSpawner.CurrentWave();
        levelSave.timeBeforNextWave = enemiesSpawner.TimeBeforeNextWave();
        levelSave.iValue = enemiesSpawner.iValue;
        levelSave.jValue = enemiesSpawner.jValue;

        foreach (GameObject enemy in enemiesSpawner.GetAliveEnnemies())
        {
            levelSave.ennemies.Add(enemy.GetComponent<Enemie>().enemyType);
            levelSave.enemiesHealth.Add(enemy.GetComponent<AIStats>().GetHealth());
            levelSave.enemiesPath.Add(enemy.GetComponent<Enemie>().GetPath());
            levelSave.ennemiesCurrentGoingWaypoints.Add(enemy.GetComponent<Enemie>().GetCurrentGoingWaypoint());
            levelSave.ennemiesPosition.Add(enemy.transform.position);
        }

        string levelSaveData = JsonUtility.ToJson(levelSave);
        File.WriteAllText(saveFilePath, levelSaveData);
    }

    public void LoadActualLevel()
    {
        playerController.SetActive(true);
        Time.timeScale = 1;
        string levelSaveData = File.ReadAllText(saveFilePath);
        LevelSaveData levelSave = new LevelSaveData();
        JsonUtility.FromJsonOverwrite(levelSaveData, levelSave);

        playerGolds.RestorGold(levelSave.gold);
        playerHealth.RestorHealth(levelSave.health);
        for (int i = 0; i < levelSave.towers.Count; i++)
        {
            towers[i].SetTowerData(levelSave.towers[i]);
        }
        enemiesSpawner.RestorData(levelSave.currentWave, levelSave.timeBeforNextWave, levelSave.iValue, levelSave.jValue);

        for (int i = 0; i < levelSave.ennemies.Count; i++)
        {
            GameObject enemy = Instantiate(enemies[levelSave.ennemies[i]]);
            enemy.transform.position = levelSave.ennemiesPosition[i];
            enemy.GetComponent<Enemie>().InitializeOnSpawn(enemiesSpawner.Waves()[levelSave.currentWave].path, enemiesSpawner, playerGolds, levelSave.ennemiesCurrentGoingWaypoints[i]);
            print(levelSave.enemiesHealth[i]);
            enemy.GetComponent<AIStats>().LoadHealth(levelSave.enemiesHealth[i]);
            enemiesSpawner.AddInEnemiesAlive(enemy);
        }

    }

    public void SetTimeScaleTo1()
    {
        Time.timeScale = 1;
        tutoPanel.SetActive(true);
    }
}
