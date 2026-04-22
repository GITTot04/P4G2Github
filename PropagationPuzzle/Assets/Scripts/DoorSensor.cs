using UnityEngine;

public class DoorSensor : CheckSound
{
    public float maximumOcclusion = 1;
    public float minimumIntensity = 0;
    public override void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        if (CalculateValues().Item1 < maximumOcclusion && CalculateValues().Item2 > minimumIntensity) // (occlusion,intensity) is returned from this method.
        {
            Debug.Log("Door opens");
        }
    }
}
