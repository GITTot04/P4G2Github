using UnityEngine;

public class Amplifier : CheckSound
{
    public int order;
    public override void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        CalculateValues(); // (occlusion,intensity) is returned from this method.
    }
}
