using FMOD.Studio;
using FMODUnity;
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
        narNameField.alpha = 0f;
        narTextField.alpha = 0f;
        maxLength = texts.Length;
        narrating = true;
        Narrator();
    }
    public void Narrator()
    {
        if (currentLength == maxLength)
        {
            narrating = false;
            Debug.Log("End");
        }
        Debug.Log("Running");
        StartCoroutine(NarrationTime());
    }
    public IEnumerator NarrationTime()
    {
        if (narrating) 
        {
            
            Debug.Log("Narrating");
            narTextField.text = texts[currentLength];
            float timer = textTimer[currentLength];
            
            currentLength++;
            while  (narTextField.alpha < 1f || narNameField.alpha < 1f)
            {
                narNameField.alpha += Time.deltaTime;
                narTextField.alpha += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(timer);
            while (narNameField.alpha > 0f || narTextField.alpha > 0f)
            {
                narTextField.alpha -= Time.deltaTime;
                narNameField.alpha -= Time.deltaTime;
                yield return null;
            }
            Narrator();
        }
        Debug.Log("We here?");

    }
}
