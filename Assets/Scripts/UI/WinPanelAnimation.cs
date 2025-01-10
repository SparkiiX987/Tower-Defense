using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanelAnimation : MonoBehaviour
{
    [SerializeField] private List<Image> stars = new();
    [SerializeField] private int level;

    void Start()
    {
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].color = Color.black;
        }
        StartCoroutine(AddStarsInUI());
    }

    private IEnumerator AddStarsInUI()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < StarsManager.Instance.GetStarInLevel(level); i++)
        {
            stars[i].color = Color.white;
            yield return new WaitForSeconds(1);
        }
    }
}
