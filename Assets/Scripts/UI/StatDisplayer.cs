using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI entityName;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI attackDamages;
    [SerializeField] private Image resistanceImage;
    [SerializeField] private Sprite resistanceSprite;
    [SerializeField] private Sprite attackSpeedSprite;
    [SerializeField] private TextMeshProUGUI resistance;
    [SerializeField] private TextMeshProUGUI other;
    [SerializeField] private Image otherImage;
    [SerializeField] private Sprite skullSprite;
    [SerializeField] private Sprite distanceSprite;

    private AIStats enemie = null;
    private Tower towerData = null;


    public void SetData(Tower data)
    {
        towerData = data;
    }

    public void SetData(AIStats data)
    {
        enemie = data;
    }

    private void DisplayStats(AIStats enemie)
    {
        this.icon.sprite = enemie.GetComponent<SpriteRenderer>().sprite;
        entityName.text = enemie.GetComponent<Enemie>().enemieName;
        health.transform.parent.gameObject.SetActive(true);
        health.text = Mathf.Round(enemie.GetHealth()) + "/" +  Mathf.Round(enemie.GetMaxHealth());
        attackDamages.text = enemie.GetDamages().ToString();
        this.other.text = enemie.GetBaseDamage().ToString();
        otherImage.sprite = skullSprite;

        resistanceImage.sprite = resistanceSprite;
        switch (enemie.GetResistance())
        {
            case damagesType.none:
                resistanceImage.color = Color.white;
                this.resistance.text = "None";
                return;

            case damagesType.magical:
                resistanceImage.color = Color.blue;
                break;

            case damagesType.physical:
                resistanceImage.color = Color.white;
                break;
        }

        this.resistance.text = "Moyen";
    }

    private void DisplayStats(Tower towerData)
    {
        icon.sprite = towerData.sprite;
        entityName.text = towerData.Name;
        health.transform.parent.gameObject.SetActive(false);
        attackDamages.text = towerData.damages.ToString();

        resistanceImage.sprite = attackSpeedSprite;
        resistanceImage.color = Color.white;
        resistance.text = towerData.attackCooldown + "/S";

        otherImage.sprite = distanceSprite;
        if (0 >= towerData.range && towerData.range < 4)
        {
            other.text = "Low";
        }
        else if (4 >= towerData.range && towerData.range < 7)
        {
            other.text = "Moyen";
        }
        else if (7 >= towerData.range && towerData.range < 9)
        {
            other.text = "Far";
        }
        else
        {
            other.text = "Super";
        }
    }

    public void ResetData()
    {
        enemie = null;
        towerData = null;
    }

    private void Update()
    {
        if (towerData)
        {
            DisplayStats(towerData);
            return;
        }

        if (enemie)
        {
            DisplayStats(enemie);
            return;
        }

        if(enemie == null && towerData == null)
        {
            gameObject.SetActive(false);
        }

        else
        {
            ResetData();
        }
    }
}
