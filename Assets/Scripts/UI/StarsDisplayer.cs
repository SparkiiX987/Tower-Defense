using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsDisplayer : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private List<Image> stars = new List<Image>();


    private void Start()
    {
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].color = Color.black;
        }

        if (!StarsManager.Instance.IsUnlocked(levelNumber))
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        for (int i = 0; i < StarsManager.Instance.GetStarInLevel(levelNumber); i++)
        {
            stars[i].color = Color.white;
        }
    }
}