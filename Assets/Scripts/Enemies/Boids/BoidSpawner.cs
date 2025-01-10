using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> stickPoints = new List<Transform>();
    private int index = 0;
    private float timer = 3f;


    [SerializeField] private GameObject prefab;
    [SerializeField] private int number;
    List<GameObject> objects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < number; i++)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
            Boids boid = go.GetComponent<Boids>();
            boid.SetSpawner(this);
            objects.Add(go);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            NextPoint();
            timer = 3f;
        }
    }

    private void NextPoint()
    {
        if (index + 1 >= stickPoints.Count)
        {
            index = 0;
            return;
        }
        index++;
    }

    public Transform GetCenterTransform()
    {
        return stickPoints[index];
    }

    public List<GameObject> GetAllEnemies() { return objects; }
}
