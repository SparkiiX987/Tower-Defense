using UnityEngine;

public class Enemie : MonoBehaviour
{
    public string enemieName;
    private Path path;
    private AIStats stats;

    private Transform selfTransform;

    private Vector2 newPosition;

    private int currentGoingWaypoint;

    private Vector2 directionToNextWaypoint;

    private EnemiesSpawner spawner;

    private PlayerGolds playerGold;

    private bool isBlocked;

    private UnitsBehaviour blocker;

    private float time;

    public int enemyType;

    public void InitializeOnSpawn(Path path, EnemiesSpawner spawner, PlayerGolds gold, int currentGoingWaypoint = 0)
    {
        stats = GetComponent<AIStats>();
        selfTransform = transform;
        this.path = path;
        this.spawner = spawner;
        this.playerGold = gold;
        this.currentGoingWaypoint = currentGoingWaypoint;
        CalculDirectionToNextWaypoint();
    }

    public void ChangeAiStats(AIStats stats)
    {
        this.stats = stats;
    }

    public void SetPath(Path path)
    {
        this.path = path;
    }

    public void SetCurrentGoingPath(int currentGoingWaypoint)
    {
        this.currentGoingWaypoint = currentGoingWaypoint;
    }

    public AIStats GetStats() { return stats; }

    public Path GetPath() { return path; }

    public int GetCurrentGoingWaypoint() {  return currentGoingWaypoint; }

    public void ChangeBlockedState()
    {
        isBlocked = !isBlocked;
    }

    public void SetBlocker(UnitsBehaviour blocker)
    {
        this.blocker = blocker;
    }

    public bool GetIsBlocked() {  return isBlocked; }

    private void Timer()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
    }

    private void Update()
    {
        if(Time.timeScale > 0)
        {
            if (!isBlocked)
            {
                if (IsAtWaypoint())
                {
                    SelectNextWaypoint();
                }

                GoToWaypoint();
            }

            if (blocker && time <= 0)
            {

            }
        }
    }

    private void SelectNextWaypoint()
    {
        if ((currentGoingWaypoint + 1) >= path.GetWayPointsNumber())
        {
            Destroy(gameObject);
            return;
        }

        currentGoingWaypoint++;
        CalculDirectionToNextWaypoint();
    }

    public float GetDistanceWithEnd()
    {
        return Vector2.Distance(selfTransform.position, path.GetWayPointAt(path.GetWayPointsNumber() - 1).position);
    }

    private bool IsAtWaypoint()
    {
        return Vector2.Distance(selfTransform.position, path.GetWayPointAt(currentGoingWaypoint).position) <= 0.5f;
    }

    private void CalculDirectionToNextWaypoint()
    {
        if(path == null) { print("path est null"); return; }
        directionToNextWaypoint = (path.GetWayPointAt(currentGoingWaypoint).position - selfTransform.position).normalized;
    }

    public Vector2 GetPositionInSecondes()
    {
        Vector2 nextPos = new Vector2 (selfTransform.position.x + (directionToNextWaypoint.x * stats.GetMoveSpeed() * 75), selfTransform.position.y + (directionToNextWaypoint.y * stats.GetMoveSpeed() * 75));
        return nextPos;
    }

    private void GoToWaypoint()
    {
        newPosition.Set(selfTransform.position.x + (directionToNextWaypoint.x * stats.GetMoveSpeed()), selfTransform.position.y + (directionToNextWaypoint.y * stats.GetMoveSpeed()));
        selfTransform.position = newPosition; ;
    }

    private void Attack()
    {
        time = stats.GetAttackCooldown();
        blocker.TakeDamages(stats.GetDamages(), stats.GetDamagesType());
    }

    private void OnDestroy()
    {
        if (stats.wasKilled) { playerGold.AddGold(stats.UnitGold()); }

        spawner.RemoveEnemie(gameObject);
    }
}
