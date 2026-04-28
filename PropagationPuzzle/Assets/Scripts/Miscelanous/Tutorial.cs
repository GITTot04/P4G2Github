using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutText;
    TextMeshProUGUI text;
    public List<string> messages = new List<string>();
    int showPosition = 0;

    void Start()
    {
        PlayerInput.instance.onNext += Next;
        PlayerInput.instance.onPrevious += Previous;
        text = tutText.GetComponent<TextMeshProUGUI>();
        UpdateText();
    }
    
    void Next()
    {
        if(showPosition < messages.Count)
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
    }
}
