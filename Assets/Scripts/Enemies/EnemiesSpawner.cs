using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [Header("wave data")]
    [SerializeField] private List<Wave> waves = new List<Wave>();
    [SerializeField] private List<float> timeBetween = new List<float>();
    [SerializeField] private Transform spawnPoint;
    private List<GameObject> enemiesAlive = new List<GameObject>();
    private int currentWave;
    private bool startWave;
    private float timeBeforNextWave;

    public int iValue { get; private set; }
    public int jValue { get; private set; }

    [Header("UIs")]
    [SerializeField] private PlayerGolds golds;
    [SerializeField] private InGameUI UI;
    [SerializeField] private GameObject nextWaveButton;
    [SerializeField] private WinLoose winLoose;

    [Header("Other")]
    [SerializeField] private PlayerHealthManager playerHealth;

    [Header("If Boss")]
    [SerializeField] private Castle castle;

    private void Start()
    {
        UI.UpdateWaveText(currentWave, waves.Count);
        startWave = false;
        timeBeforNextWave = timeBetween[0];
        nextWaveButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, waves[currentWave].buttonWorldPos.position);
    }

    public int GetCurrentWave() { return currentWave; }

    public int CurrentWave() { return currentWave - 1; }
    public int TimeBeforeNextWave() { return (int)timeBeforNextWave; }

    public void AddInEnemiesAlive(GameObject addedEnemy)
    {
        enemiesAlive.Add(addedEnemy);
    }

    public void RestorData(int wave, int timeBeforNextWave, int iValue, int jValue)
    {
        print(wave);
        currentWave = wave;
        if (wave < 0) { wave = 0; }
        this.timeBeforNextWave = timeBeforNextWave;
        StartCoroutine(SpawnWave(iValue, jValue + 1));
    }

    private void Timer()
    {
        if (timeBeforNextWave < 0) { return; }
        timeBeforNextWave -= Time.deltaTime;
        UI.UpdateTimeText((int)timeBeforNextWave);
    }

    private void Update()
    {
        if (!(currentWave < timeBetween.Count))
        { return; }

        Timer();

        if (!nextWaveButton.activeInHierarchy && (timeBetween[currentWave] / 2) >= timeBeforNextWave && timeBeforNextWave > 0)
        {
            nextWaveButton.SetActive(true);
            nextWaveButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, waves[currentWave].buttonWorldPos.position);
        }

        if (startWave || (timeBeforNextWave <= 0 && !(timeBetween[currentWave] == -10)))
        {
            startWave = false;
            StartCoroutine(SpawnWave());
            currentWave++;
            UI.UpdateWaveText(currentWave, waves.Count);
            if (currentWave < timeBetween.Count)
            {
                timeBeforNextWave = timeBetween[currentWave];
            }
            if (currentWave >= waves.Count)
            {
                nextWaveButton.SetActive(false);
                UI.UpdateTimeText(-1);
            }
        }
    }

    public List<Wave> Waves() { return waves; }

    private IEnumerator SpawnWave(int iValue = 0, int jValue = 0)
    {
        nextWaveButton.SetActive(false);
        int wave = currentWave;
        if(wave == waves.Count - 1 && castle)
        {
            castle.PlayAnimation();
        }
        for (int i = iValue; i < waves[wave].enemiesGameObject.Count; i++)
        {
            this.iValue = i;
            for (int j = jValue; j < waves[wave].enemiesNumber[i]; j++)
            {
                GameObject go = Instantiate(waves[wave].enemiesGameObject[i]);
                go.transform.position = waves[wave].path.GetWayPointAt(0).position;
                go.GetComponent<Enemie>().InitializeOnSpawn(waves[wave].path, this, golds);
                enemiesAlive.Add(go);
                this.jValue = j;
                yield return new WaitForSeconds(waves[wave].enemiesSpawnCooldown);
            }
        }
        if (IsAtLastWave())
        {
            while (true)
            {
                if (HasNoEnnemieRemaining())
                {
                    yield return new WaitForSeconds(2);
                    int StarsWin = 3;
                    if (playerHealth.GetHealth() < 20)
                    {
                        StarsWin = playerHealth.GetHealth() > 19 ? 2 : playerHealth.GetHealth() > 12 ? 1 : 0;
                    }
                    winLoose.InvokeWinEvent(StarsWin);
                    break;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }

    public bool IsAtLastWave() { return (currentWave == waves.Count); }
    public bool HasNoEnnemieRemaining() { return (enemiesAlive.Count == 0); }

    public List<GameObject> GetAliveEnnemies() { return enemiesAlive; }

    public void RemoveEnemie(GameObject enemieGO)
    {
        enemiesAlive.Remove(enemieGO);
    }

    public void StartWave()
    {
        startWave = true;
        if ((int)timeBetween[currentWave] > 0)
        {
            golds.AddGold((int)timeBetween[currentWave]);
        }
    }
}
