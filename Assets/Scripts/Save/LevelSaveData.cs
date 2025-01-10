using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public int health;
    public int gold;
    public List<Tower> towers = new List<Tower>();
    public int currentWave;
    public int timeBeforNextWave;
    public int iValue;
    public int jValue;
    public List<int> ennemies = new List<int>();
    public List<Vector3> ennemiesPosition = new List<Vector3>();
    public List<Path> enemiesPath = new List<Path>();
    public List<int> ennemiesCurrentGoingWaypoints = new List<int>();
    public List<float> enemiesHealth = new List<float>();
}