using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject impactParticles;
    [SerializeField] private bool curveMovement;
    [SerializeField] private AnimationCurve curve;
    [Header("Only if is mortar bullet")]
    [SerializeField] private float ExplosionRange;
    [SerializeField] private LayerMask EnemyMask;
    private float damages;
    private damagesType damagesType;

    private Transform selfTransform;

    private Transform targetTransform;

    private Vector2 newPosition;
    private Vector2 targetDirection = Vector2.zero;

    private Capacity capacity;

    private Vector3 start;
    private Coroutine coroutine;

    private bool HasApplyDamagesTotarget;

    private void Start()
    {
        selfTransform = transform;
        start = selfTransform.position;

        if (curveMovement)
        {
            coroutine = StartCoroutine(Curve());
        }
    }

    public void Initialize(float damages, damagesType damagesType, Transform targetTransform, Capacity capacity)
    {
        this.damages = damages;
        this.damagesType = damagesType;
        this.targetTransform = targetTransform;
        this.capacity = capacity;
    }

    private void Update()
    {
        if (coroutine != null) { return; }
        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        BulletBehaviour();
        return;
    }

    private void BulletBehaviour()
    {
        CalculTargetDirection();
        BulletMove();

        if (IsOnEnemie())
        {
            Impact();
        }
    }



    private IEnumerator Curve()
    {
        float duration = 0.40f;
        float time = 0f;

        Vector3 end = targetTransform.GetComponent<Enemie>().GetPositionInSecondes() - (Vector2)(targetTransform.forward * 0.55f);

        while (time < duration)
        {
            time += Time.deltaTime;

            float linearT = time / duration;
            float heightT = curve.Evaluate(linearT);


            float height = Mathf.Lerp(0f, 5.0f, heightT);

            selfTransform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }

        if (targetTransform == null)
        {
            Destroy(gameObject);
            coroutine = null;
            yield break;
        }

        if (IsOnEnemie())
        {
            Impact();
        }
        else if (capacity == Capacity.AOE)
        {
            AOEDamages();
        }
        coroutine = null;
        yield break;
    }


    private void Impact()
    {
        PlayImpactParticles();
        ApplyDamages();
    }

    private void CalculTargetDirection()
    {
        targetDirection = (targetTransform.position - selfTransform.position).normalized;
    }

    private void BulletMove()
    {
        newPosition.Set(selfTransform.position.x + (targetDirection.x * bulletSpeed * Time.deltaTime), selfTransform.position.y + (targetDirection.y * bulletSpeed * Time.deltaTime));
        selfTransform.position = newPosition;
    }

    private bool IsOnEnemie() { return Vector2.Distance(selfTransform.position, targetTransform.position) <= 0.5f; }

    private void ApplyDamages()
    {
        targetTransform.GetComponent<AIStats>().TakeDamages(damages, damagesType);
        HasApplyDamagesTotarget = true;
        if (capacity == Capacity.AOE)
        {
            AOEDamages();
        }
        Destroy(gameObject);
    }

    private void AOEDamages()
    {
        PlayImpactParticles();
        RaycastHit2D[] otherEnemies = Physics2D.CircleCastAll(selfTransform.position, ExplosionRange, Vector2.up, ExplosionRange, EnemyMask);
        for (int i = 0; i < otherEnemies.Length; i++)
        {
            if (HasApplyDamagesTotarget && otherEnemies[i].transform == targetTransform) { continue; }
            otherEnemies[i].transform.GetComponent<AIStats>().TakeDamages(damages, damagesType);
        }
    }

    private void PlayImpactParticles()
    {
        GameObject particles = Instantiate(impactParticles);
        particles.transform.position = selfTransform.position;
        Destroy(particles, 1f);
    }
}
