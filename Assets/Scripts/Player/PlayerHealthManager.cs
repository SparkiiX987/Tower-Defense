using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    [SerializeField] private InGameUI healthUI;
    [SerializeField] private WinLoose winLoose;

    public int GetHealth() { return healthPoints; }

    private void Start()
    {
        healthUI.UpdateHealthPointsText(healthPoints);
    }

    public void RestorHealth(int health)
    {
        healthPoints = health;
        healthUI.UpdateHealthPointsText(health);
    }

    public void TakeDamages(int damages)
    {
        healthPoints -= damages;
        if(healthPoints <= 0)
        {
            healthPoints = 0;
            winLoose.InvokeLooseEvent(0);
        }
        healthUI.UpdateHealthPointsText(healthPoints);
    }
}
