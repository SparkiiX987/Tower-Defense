using UnityEngine;

public class AIStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float damages;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int baseDamage;
    [SerializeField] private damagesType damageType;
    [SerializeField] private damagesType resistance;
    [SerializeField] private int goldGiven;
    [SerializeField] HealthBar healthBar;

    private float health;
    public bool wasKilled;

    public float GetHealth() {  return health; }
    public float GetMaxHealth() {  return maxHealth; }
    public int UnitGold() { return goldGiven; }
    public float GetMoveSpeed() { return movementSpeed; }
    public int GetBaseDamage() { return baseDamage; }
    public float GetAttackCooldown() { return attackCooldown; }
    public damagesType GetDamagesType() { return damageType; }
    public damagesType GetResistance() { return resistance; }
    public float GetDamages() { return damages; }

    [HideInInspector] public bool isLoaded;

    private void Start()
    {
        if(isLoaded) { return; }
        health = maxHealth;
        healthBar.UpdateHealthBar(maxHealth, health);
    }

    public void LoadHealth(float health)
    {
        this.health = health;
        healthBar.UpdateHealthBar(maxHealth, health);
        isLoaded = true;
    }

    public void TakeDamages(float damages, damagesType damagesType)
    {
        if(damagesType == resistance) { damages *= 0.8f; }

        if (damages >= health)
        {
            wasKilled = true;
            Destroy(gameObject);
            return;
        }
        health -= damages;
        healthBar.UpdateHealthBar(maxHealth, health);
    }

    public void Heal(float Amount)
    {
        if(health + Amount > maxHealth)
        {
            health = maxHealth;
            return;
        }
        health += Amount;
        healthBar.UpdateHealthBar(maxHealth, health);
    }
}
