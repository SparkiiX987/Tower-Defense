using UnityEngine;

public class StarsManager : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private int levelNumber;
    public static StarsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        InitializeLevels();
    }

    private void InitializeLevels()
    {
        for (int i = 1; i < levelNumber + 1; i++)
        {
            if (PlayerPrefs.GetInt("Unlocked" + LevelName(i)) != 1)
            {
                PlayerPrefs.SetInt("Unlocked" + LevelName(i), 0);
            }
        }
        PlayerPrefs.SetInt("Unlocked" + LevelName(1), 1);
    }

    private string LevelName(int levelNumber)
    {
        return levelName + levelNumber;
    }

    public void AddStars(int amount, int level)
    {
        if(PlayerPrefs.GetInt(LevelName(level)) >= amount) { return; }

        int difference = amount - PlayerPrefs.GetInt(LevelName(level));

        SetStarInLevel(amount, level);
        PlayerPrefs.SetInt("stars", PlayerPrefs.GetInt("stars") + difference);

    }

    public bool LevelExist(int level)
    {
        return PlayerPrefs.HasKey("Unlocked" + LevelName(level));
    }

    public void UnlockLevel(int level)
    {
        PlayerPrefs.SetInt("Unlocked" + LevelName(level), 1);
    }

    public bool IsUnlocked(int level)
    {
        return PlayerPrefs.GetInt("Unlocked" + LevelName(level)) == 1;
    }

    public int GetStars()
    {
        return PlayerPrefs.GetInt("stars");
    }

    public void SetStarInLevel(int amount, int level)
    {
        if(amount < PlayerPrefs.GetInt(LevelName(level))) { return; }

        PlayerPrefs.SetInt(LevelName(level), amount);
    }

    public int GetStarInLevel(int level)
    {
        return PlayerPrefs.GetInt(LevelName(level));
    }

    public void ResetProgression()
    {
        PlayerPrefs.DeleteAll();
        InitializeLevels();
    }
}
