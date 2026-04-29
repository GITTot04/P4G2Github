using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutText;
    TextMeshProUGUI text;
    public List<string> messages = new List<string>();
    int showPosition = 0;

    public GameObject nextText;
    public GameObject prevText;

    void Start()
    {
        PlayerInput.instance.onNext += Next;
        PlayerInput.instance.onPrevious += Previous;
        text = tutText.GetComponent<TextMeshProUGUI>();
        UpdateText();
    }
    
    void Next()
    {
        if (showPosition < messages.Count - 1)
        {
            showPosition++;
        }
        UpdateText();
    }
    void Previous()
    {
        if(showPosition > 0)
        {
            showPosition--;
        }
        UpdateText();
    }

    void UpdateText ()
    {
        text.text = messages[showPosition];

        if (showPosition == messages.Count - 1)
        {
            nextText.GetComponent<TextMeshProUGUI>().faceColor = new Color32(0, 0, 0, 0);
        }
        else
        {
            nextText.GetComponent<TextMeshProUGUI>().faceColor = new Color32(0, 0, 0, 255);

        }
        if(showPosition == 0)
        {

            prevText.GetComponent<TextMeshProUGUI>().faceColor = new Color32(0, 0, 0, 0);
        }
        else
        {
            prevText.GetComponent<TextMeshProUGUI>().faceColor = new Color32(0, 0, 0, 255);
        }
    }
}
