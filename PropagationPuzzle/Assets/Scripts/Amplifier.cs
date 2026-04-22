using UnityEngine;

public class Amplifier : CheckSound
{
    public int order;
    public float amplifierOcclusion;
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
        amplifierOcclusion = CalculateValues().Item1; // (occlusion,intensity) is returned from this method.
    }
}
