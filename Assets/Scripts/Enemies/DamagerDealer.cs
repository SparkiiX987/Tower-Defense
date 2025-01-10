using UnityEngine;

public class DamagerDealer : MonoBehaviour
{
    [SerializeField] private PlayerHealthManager playerHealthManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<AIStats>())
        playerHealthManager.TakeDamages(collision.GetComponent<AIStats>().GetBaseDamage());
    }
}
