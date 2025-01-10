using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerConstructer : MonoBehaviour
{
    [SerializeField] private PlayerGolds playerGolds;
    [SerializeField] private List<GameObject> buttons = new List<GameObject>();
    [SerializeField] private Transform worldPositionObject;
    [SerializeField] private Transform towerRangeDisplayer;
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    [SerializeField] private Sprite sellSprite;

    private Vector3 newWorldObectPosition;
    private List<Tower> constructibleTower = new List<Tower>();
    private TowerBehaviour tower;
    private RectTransform selfTransform;

    private void Awake()
    {
        selfTransform = GetComponent<RectTransform>();
        playerGolds.OnGoldUpdate.AddListener(UpdateButtonsVisual);
    }

    private void OnDisable()
    {
        if (worldPositionObject != null)
        {
            worldPositionObject.gameObject.SetActive(false);
        }
        if (towerRangeDisplayer != null)
        {
            towerRangeDisplayer.gameObject.SetActive(false);
        }
    }

    public void Construct(int index)
    {
        if (constructibleTower[index].coast == 0)
        {
            int goldsRefund = enemiesSpawner.GetCurrentWave() <= 0 ? tower.GetTowerData().refundCoast : tower.GetTowerData().refundCoast / 2;
            Sell(goldsRefund, index);
        }

        if (playerGolds.GetGold() < constructibleTower[index].coast) { return; }

        tower.SetTowerData(constructibleTower[index]);
        playerGolds.UseGold(constructibleTower[index].coast);

        gameObject.SetActive(false);
    }

    private void Sell(int gold, int index)
    {
        playerGolds.AddGold(gold);
        tower.SetTowerData(constructibleTower[index]);
    }

    public void Initialize(TowerBehaviour tower)
    {
        this.tower = tower;
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetActive(false);
        }

        UpdateButtonsVisual();

        worldPositionObject.gameObject.SetActive(true);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(selfTransform, selfTransform.position, Camera.main, out newWorldObectPosition);
        worldPositionObject.position = newWorldObectPosition;

        towerRangeDisplayer.gameObject.SetActive(true);
        towerRangeDisplayer.position = newWorldObectPosition;
        towerRangeDisplayer.localScale = new Vector2(0.125f, 0.125f) * this.tower.GetTowerData().range;
    }

    private void UpdateButtonsVisual()
    {
        constructibleTower = tower.GetNextTowersList();
        for (int i = 0; i < constructibleTower.Count; i++)
        {
            buttons[i].SetActive(true);
            if (constructibleTower[i].refundCoast == 0)
            {
                buttons[i].GetComponent<Image>().sprite = sellSprite;
                buttons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                buttons[i].GetComponent<Image>().sprite = constructibleTower[i].sprite;
                buttons[i].GetComponent<Image>().color = Color.white;
                buttons[i].transform.GetChild(0).gameObject.SetActive(true);
                buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = constructibleTower[i].coast.ToString();
                if (playerGolds.GetGold() < constructibleTower[i].coast)
                {
                    buttons[i].GetComponent<Image>().color = Color.grey;
                }
            }
        }
    }
}
