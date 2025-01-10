using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tuto : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tuto;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject startNextWaveButton;
    [SerializeField] private GameObject playerController;
    [SerializeField] private List<string> texts = new List<string>();
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    private int currentIndex = 0;

    public int textsCount {  get { return texts.Count; } }

    private void Start()
    {
        if(texts.Count == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        DisplayText();
    }

    private void DisplayText()
    {
        startNextWaveButton.SetActive(false);
        tuto.text = texts[currentIndex];
        DisplaySprite();
    }

    private void DisplaySprite()
    {
        if (sprites[currentIndex] == null)
        {
            image.color = new Color(0, 0, 0, 0);
        }
        else 
        {
            image.sprite = sprites[currentIndex];
            image.color = Color.white;
        }
        
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex == texts.Count)
        {
            startNextWaveButton.SetActive(true);
            playerController.SetActive(true);
            gameObject.SetActive(false);
            return;
        }
        else if (currentIndex == texts.Count - 1)
        {
            buttonText.text = "Finish";
        }

        DisplayText();
    }
}
