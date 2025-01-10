using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;

    public void UpdateHealthBar(float maxHealth, float health)
    {
        healthSlider.value = health/maxHealth;
    }
}
