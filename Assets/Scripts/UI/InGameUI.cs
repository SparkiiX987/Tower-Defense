using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI timeBeforNextWave;
    [SerializeField] private TextMeshProUGUI HealthPoints;
    [SerializeField] private TextMeshProUGUI Wave;

    private void Start()
    {
        timeBeforNextWave.text = "Start the first wave";
    }

    public void UpdateGoldText(int newGoldAmount)
    {
        gold.text = "golds : " + newGoldAmount.ToString();
    }

    public void UpdateWaveText(int currentWave, int maxWave)
    {
        Wave.text = "Wave : " + currentWave + " / " + maxWave;
    }

    public void UpdateTimeText(int timeInSecond)
    {
        if(timeInSecond == -1) 
        {
            timeBeforNextWave.text = "No waves remaining";
            return;
        }
        timeBeforNextWave.text = "Next wave in " + timeInSecond + " seconds";
    }

    public void UpdateHealthPointsText(int health)
    {
        HealthPoints.text = health.ToString();
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
