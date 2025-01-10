using UnityEngine;

public class UnitsBehaviour : MonoBehaviour
{
    [Header("stats")]
    [SerializeField] private float healthPoints;
    [SerializeField] private float maxHealthPoints;
    [SerializeField] private float damages;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float movementSpeed;
    [SerializeField] private damagesType damageType;
    [SerializeField] private damagesType resistance;

    [Header("Agro")]
    [SerializeField] private float agroRange;
    [SerializeField] private LayerMask enemiesLayer;

    private Transform selfTransform;

    [SerializeField] private Vector2 ralliPoint = Vector2.zero;

    private Vector2 direction = Vector2.zero;
    private Vector2 newPosition = Vector2.zero;

    private AIStats enemieStats;

    private float time;

    public void TakeDamages(float damages, damagesType damagesType)
    {
        if (damagesType == resistance) { damages *= 0.8f; }

        if (damages >= healthPoints)
        {
            Destroy(gameObject);
            return;
        }
        healthPoints -= damages;
    }

    private void Start()
    {
        selfTransform = transform;
    }

    public void InitializeUnit(Vector2 ralliPoint, float health, float damage)
    {
        this.ralliPoint = ralliPoint;
        maxHealthPoints = health;
        healthPoints = health;
        damages = damage;
    }

    private void Update()
    {
        Timer();

        if (enemieStats == null)
        {
            CheckOfEnnemies();
        }
        if (time <= 0 && enemieStats != null)
        {
            Attack();
        }
        SetDirection();
        Movement();
    }

    private void Timer()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
    }

    #region Behaviour

    private void SetDirection()
    {
        if (enemieStats)
        {
            direction.Set(enemieStats.transform.position.x - selfTransform.position.x, enemieStats.transform.position.y - selfTransform.position.y);
        }
        else
        {
            direction.Set(ralliPoint.x - selfTransform.position.x, ralliPoint.y - selfTransform.position.y);
        }
        direction.Normalize();
    }

    private void Movement()
    {
        newPosition.Set(selfTransform.position.x + direction.x * movementSpeed, selfTransform.position.y + direction.y * movementSpeed);
        print(transform.position + " -> " + newPosition);
        transform.position = newPosition;
    }

    private void CheckOfEnnemies()
    {
        RaycastHit2D[] ennemies = Physics2D.CircleCastAll(selfTransform.position, agroRange, Vector2.zero, agroRange, enemiesLayer);
        Enemie ennemie = null;

        for (int i = 0; i < ennemies.Length; i++)
        {
            if (ennemie == null) { ennemie = ennemies[i].transform.GetComponent<Enemie>(); }

            if (ennemie.GetDistanceWithEnd() < ennemies[i].transform.GetComponent<Enemie>().GetDistanceWithEnd())
            {
                ennemie = ennemies[i].transform.GetComponent<Enemie>();
            }
        }

        if(ennemie == null) { return; }

        ennemie.ChangeBlockedState();
        enemieStats = ennemie.GetComponent<AIStats>();
    }

    private void Attack()
    {
        time = attackCooldown;
        enemieStats.TakeDamages(damages, damageType);
    }

    private void OnDestroy()
    {
        if (enemieStats != null)
        {
            enemieStats.GetComponent<Enemie>().ChangeBlockedState();
        }
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroRange);

        if (ralliPoint != null)
        {
            Gizmos.DrawSphere(ralliPoint, 0.1f);
        }
    }
}
