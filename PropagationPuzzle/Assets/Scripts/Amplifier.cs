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
    private void Start()
    {
        FindOcclusionAndIntensity();
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
        }
        else
        {
            isAmplifying = false;
        }
    }
}
