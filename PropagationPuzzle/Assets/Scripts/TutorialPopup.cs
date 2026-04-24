using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    public TextMeshProUGUI tutText;
    public Button nextButton;
    public Button backButton;
    public GameObject tutHolder;
    public int pageStage;
    public string text1;
    public string text2;
    public string text3;
    public void Start()
    {
        tutHolder.SetActive(true);
    }
    public void NextButton() 
    { 
        pageStage++;
    }
    public void BackButton()
    {
        pageStage--;
    }
    public void CloseButton()
    {
        tutHolder.SetActive(false);
    }
    public void Update()
    {
        switch (pageStage)
        {
            case 0:
                backButton.gameObject.SetActive(false);

                tutText.text = text1;
                break;
            case 1:
                backButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);
                tutText.text = text2;
                break;
            case 2:
                nextButton.gameObject.SetActive(false);
                tutText.text = text3;
                break;

        }
    }
}
