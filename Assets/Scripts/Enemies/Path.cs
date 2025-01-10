using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path : MonoBehaviour
{
    [SerializeField] private List<Transform> pathPoints = new List<Transform>();


    public Transform GetWayPointAt(int index)
    {
        return pathPoints[index];
    }

    public int GetWayPointsNumber() { return pathPoints.Count; }
}
