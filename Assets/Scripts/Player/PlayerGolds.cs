using UnityEngine;
using UnityEngine.Events;


public class PlayerGolds : MonoBehaviour
{
    [SerializeField] private int gold;
    [SerializeField] private InGameUI UI;
    [HideInInspector] public UnityEvent OnGoldUpdate;

    private void Start()
    {
        UI.UpdateGoldText(gold);
    }

    public int GetGold()
    {
        return gold;
    }

    public void RestorGold(int gold)
    {
        this.gold = gold;
        UI.UpdateGoldText(gold);
    }

    public bool UseGold(int goldUsed)
    {
        if(goldUsed > gold) { return false; }

        gold -= goldUsed;
        UI.UpdateGoldText(gold);
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UI.UpdateGoldText(gold);
        OnGoldUpdate.Invoke();
    }
}
