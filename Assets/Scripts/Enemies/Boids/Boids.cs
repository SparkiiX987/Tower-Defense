using UnityEngine;

public class Boids : MonoBehaviour
{
    [Header("Boid Settings")]
    [SerializeField] private BoidSpawner spawner;
    [SerializeField] private float _maxForce = 1f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _separationDistance = 2f;
    [SerializeField] private float _cohesionDistance = 5f;
    [SerializeField] private float _alignmentDistance = 5f;
    [SerializeField] private float _centerAttractionStrength = 1.0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 steering;
    private Vector3 newPos;

    void Start()
    {
        velocity = new Vector3(Random.Range(-_maxSpeed, _maxSpeed), Random.Range(-_maxSpeed, _maxSpeed), 0);
    }

    void Update()
    {
        BoidsSystem();
    }

    private void BoidsSystem()
    {
        steering = Vector3.zero;

        steering += Separation() * 1.5f;
        steering += Alignment() * 1.0f;
        steering += Cohesion() * 1.0f;
        steering += AttractToCenter() * _centerAttractionStrength;

        steering = Vector3.ClampMagnitude(steering, _maxForce);

        velocity = Vector3.ClampMagnitude(velocity + steering, _maxSpeed);

        newPos.Set(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, velocity.z * Time.deltaTime);
        transform.position += newPos;
    }

    public void SetSpawner(BoidSpawner spawner)
    {
        this.spawner = spawner;
    }

    private Vector3 Alignment()
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject enemy in spawner.GetAllEnemies())
        {
            if (enemy != gameObject && Vector3.Distance(enemy.transform.position, transform.position) < _alignmentDistance)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                steer += enemy.GetComponent<Boids>().velocity / distance;
                count++;
            }
        }

        if (count > 0)
            steer /= count;

        return steer.normalized;
    }

    private Vector3 Cohesion()
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject enemy in spawner.GetAllEnemies())
        {
            if (enemy != gameObject && Vector3.Distance(enemy.transform.position, transform.position) < _cohesionDistance)
            {
                steer += enemy.transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            steer /= count;
            steer = (steer - transform.position).normalized;
        }

        return steer;
    }

    private Vector3 Separation()
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject enemy in spawner.GetAllEnemies())
        {
            if (enemy != gameObject && Vector3.Distance(enemy.transform.position, transform.position) < _separationDistance)
            {
                Vector3 diff = transform.position - enemy.transform.position;
                steer += diff.normalized / Vector3.Distance(enemy.transform.position, transform.position);
                count++;
            }
        }

        if (count > 0)
            steer /= count;

        return steer;
    }

    private Vector3 AttractToCenter()
    {
        if (spawner == null || spawner.GetCenterTransform() == null)
            return Vector3.zero;

        Vector3 centerPosition = spawner.GetCenterTransform().position;
        Vector3 desiredDirection = centerPosition - transform.position;

        return desiredDirection.normalized;
    }
}