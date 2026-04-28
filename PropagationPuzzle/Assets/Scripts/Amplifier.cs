using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Amplifier : CheckSound
{
    public int order;
    public float amplifierOcclusion;
    public bool isAmplifying;

    public GameObject ampText;
    TextMeshProUGUI text;
    public List<GameObject> colorLights = new List<GameObject>();

    private void OnEnable()
    {

        SoundManager.instance.activeAmplifiers.Add(this);
        
    }
    private void OnDisable()
    {
        SoundManager.instance.activeAmplifiers.Remove(this);
    }
    private void Start()
    {
        FindOcclusionAndIntensity();
        text = ampText.GetComponent<TextMeshProUGUI>();
        text.text = (order + 1).ToString();
    }
    public override void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        (float, float) calculatedValue = CalculateValues(); // (occlusion,intensity) is returned from this method.
        amplifierOcclusion = calculatedValue.Item1 * rayStats.MaxOcclusions; // This value is divided by MaxOcclusions for a different purpose but here it needs to be the prior value
        if (calculatedValue.Item2 > 0)
        {
            isAmplifying = true;
            UpdateLights();


        }
        else
        {
            isAmplifying = false;
            UpdateLights();     
        }
    }

    void UpdateLights()
    {
        foreach (GameObject light in colorLights)
        {
            if (isAmplifying)
            {
                light.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 255);
            } else
            {
                light.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
            }
        }
    }

}
