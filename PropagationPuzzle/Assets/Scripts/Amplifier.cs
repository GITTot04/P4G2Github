using UnityEngine;

public class Amplifier : CheckSound
{
    public int order;
    public float amplifierOcclusion;
    public bool isAmplifying;
    private void OnEnable()
    {
        SoundManager.instance.activeAmplifiers.Add(this);
    }
    private void OnDisable()
    {
        SoundManager.instance.activeAmplifiers.Remove(this);
    }
    public override void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        (float, float) calculatedValue = CalculateValues(); // (occlusion,intensity) is returned from this method.
        amplifierOcclusion = calculatedValue.Item1; 
        if (calculatedValue.Item2 > 0)
        {
            isAmplifying = true;
        }
        else
        {
            isAmplifying = false;
        }
    }
}
