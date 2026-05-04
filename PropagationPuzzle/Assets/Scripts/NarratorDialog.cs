using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NarratorDialog : MonoBehaviour
{
    private int maxLength;
    private int currentLength = 0;
    public bool narrating;
    public string[] texts;
    public float[] textTimer;
    public string nameText;
    public TextMeshProUGUI narNameField;
    public TextMeshProUGUI narTextField;

    public void Start()
    {
        //currentLength++;
        narNameField.text = nameText;
        narTextField.text = texts[currentLength];
        //narNameField.alpha = 0f;
        maxLength = texts.Length;
        narrating = true;
        Narrator();
    }
    public void Narrator()
    {
        if (currentLength == maxLength)
        {
            narrating = false;
        }
        Debug.Log("Running");
        StartCoroutine(NarrationTime());
    }
    public IEnumerator NarrationTime()
    {
        if (narrating) 
        { 
            narTextField.text = texts[currentLength];
            float timer = textTimer[currentLength];
            yield return new WaitForSeconds(timer);
            currentLength++;
            Narrator();
        }
        Debug.Log("We here?");

    }
}
