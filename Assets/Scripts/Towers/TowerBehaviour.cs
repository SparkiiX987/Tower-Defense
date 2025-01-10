using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask enemiesLayer;
    [SerializeField] private Transform arrivalTransform;

    [SerializeField] private Tower towerData;

    [SerializeField] private Transform bulletStart;

    private AudioSource audioSource;

    private float cooldown;

    private Transform targetTransfom;

    private SpriteRenderer spriteRenderer;
    private Transform selfTransform;

    [SerializeField] private LayerMask pathMask;
    [SerializeField] private Vector2 UnitsRallimentPoint = Vector2.zero;
    private List<UnitsBehaviour> units = new();

    public void SetTowerData(Tower newTowerData)
    {
        towerData = newTowerData;
        spriteRenderer.sprite = towerData.sprite;
        if (towerData.shootSFX != null)
        {
            audioSource.clip = towerData.shootSFX;
        }

        if (towerData is DefenseTower)
        {
            SetUnitsRalliPoint();
            print(UnitsRallimentPoint);
            for (int i = 0; i < 3; i++)
            {
                SpawnUnit();
            }
        }
    }

    public Tower GetTowerData() { return towerData; }

    public List<Tower> GetNextTowersList()
    {
        return towerData.NextTowers;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = towerData.sprite;
        selfTransform = transform;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (towerData is DefenseTower)
        {

            return;
        }

        CheckForEnemies();

        if (targetTransfom != null && cooldown <= 0)
        {
            if (towerData.shootSFX != null)
            {
                audioSource.Play();
            }
            Shoot();
            cooldown = towerData.attackCooldown;
        }

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    #region Shooting tower

    private void Shoot()
    {
        if (!(towerData is AttackTower)) { return; }

        AttackTower attackTowerData = (AttackTower)towerData;
        GameObject bullet = Instantiate(towerData.bulletPrefab);
        bullet.transform.position = bulletStart.position;

        bullet.GetComponent<Bullet>().Initialize(towerData.damages, towerData.damagesType, targetTransfom, attackTowerData.capacity);
    }

    private void CheckForEnemies()
    {
        if (towerData == null) { return; }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(selfTransform.position, towerData.range, Vector2.zero, 1, enemiesLayer);

        if (hits.Length == 0)
        {
            ResetTarget();
            return;
        }

        if (Vector2.Distance(selfTransform.position, hits[0].transform.position) <= towerData.range)
        {
            SetTarget(hits[0].collider);
        }
        if (hits.Length == 1) { return; }
        float distance = Vector2.Distance(selfTransform.position, hits[0].transform.position);

        for (int i = 1; i < hits.Length; i++)
        {
            if (Vector2.Distance(selfTransform.position, hits[i].transform.position) < distance)
            {
                SetTarget(hits[i].collider);
                distance = Vector2.Distance(selfTransform.position, targetTransfom.position);
            }
        }
    }

    private void SetTarget(Collider2D targetCollider)
    {
        targetTransfom = targetCollider.transform;
    }

    private void ResetTarget()
    {
        targetTransfom = null;
    }
    #endregion

    #region Defense tower

    public void SetUnitRalli(Vector2 ralliPoint)
    {
        UnitsRallimentPoint = ralliPoint;
    }

    private void SpawnUnit()
    {
        print("spawn");
        DefenseTower defenseTowerData = (DefenseTower)towerData;

        GameObject newUnit = Instantiate(defenseTowerData.unitPrefab);
        newUnit.transform.position = selfTransform.position;
        UnitsBehaviour unitBehaviour = newUnit.GetComponent<UnitsBehaviour>();
        unitBehaviour.InitializeUnit(UnitsRallimentPoint, defenseTowerData.unitHealthPoints, defenseTowerData.damages);
        units.Add(unitBehaviour);
    }

    private void SetUnitsRalliPoint()
    {
        for (float x = -1; x < 1; x += 0.1f)
        {
            for (float y = -1; y < 1; y += 0.1f)
            {
                Vector2 raycastPosition = new Vector2(transform.position.x + x, transform.position.y + y);
                if (Physics2D.Raycast(raycastPosition, Vector2.zero, Mathf.Infinity, pathMask))
                {
                    UnitsRallimentPoint = raycastPosition;
                    return;
                }
            }
        }
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        if (towerData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, towerData.range);
        }

        if (targetTransfom != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetTransfom.position);
        }
    }

}
