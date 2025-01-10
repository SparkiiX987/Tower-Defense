using UnityEngine;
using UnityEngine.Events;

public class WinLoose : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private int level;

    private UnityEvent OnWinEvent = new UnityEvent();
    private UnityEvent OnLooseEvent = new UnityEvent();


    private void Start()
    {
        OnWinEvent.AddListener(ActivateWinPanel);
        OnLooseEvent.AddListener(ActivateLoosePanel);
    }

    public void InvokeWinEvent(int amount)
    {
        StarsManager.Instance.AddStars(amount, level);
        if (StarsManager.Instance.LevelExist(level + 1))
        {
            StarsManager.Instance.UnlockLevel(level + 1);
        }
        OnWinEvent.Invoke();
    }

    public void InvokeLooseEvent(int StarsWin)
    {
        OnLooseEvent.Invoke();
    }

    private void ActivateWinPanel()
    {
        winPanel.SetActive(true);
    }

    private void ActivateLoosePanel()
    {
        loosePanel.SetActive(true);
    }
}
