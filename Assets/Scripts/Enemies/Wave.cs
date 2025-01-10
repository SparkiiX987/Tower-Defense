using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<GameObject> enemiesGameObject = new List<GameObject>();
    public List<int> enemiesNumber = new List<int>();
    public float enemiesSpawnCooldown;
    public Path path;
    public Transform buttonWorldPos;
}
