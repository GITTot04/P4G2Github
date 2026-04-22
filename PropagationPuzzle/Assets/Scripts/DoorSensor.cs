using UnityEngine;

public class DoorSensor : CheckSound
{
    public float maximumOcclusion = 1;
    public float minimumIntensity = 0;
    public ExitDoor[] exitDoors = new ExitDoor[2];
    public override void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        if (CalculateValues().Item1 < maximumOcclusion && CalculateValues().Item2 > minimumIntensity) // (occlusion,intensity) is returned from this method.
        {
            foreach (ExitDoor exitDoor in exitDoors)
            {
                exitDoor.UnlockDoor();
            }
        }
    }
}
