using UnityEngine;
using System.Collections.Generic;
using FMOD.Studio;

public class DoorSensor : CheckSound
{
    public float maximumOcclusion = 1;
    public float minimumIntensity = 0;
    public ExitDoor[] exitDoors = new ExitDoor[2];
    public List<GameObject> sensorLights = new List<GameObject>();

    public bool doorCountExceeded = false;
    public override void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        (float, float) calculate = CalculateValues();
        if (calculate.Item1 < maximumOcclusion && calculate.Item2 > minimumIntensity && !doorCountExceeded) // (occlusion,intensity) is returned from this method.
        {
            foreach (ExitDoor exitDoor in exitDoors)
            {
                exitDoor.UnlockDoor();
            }
            foreach (GameObject sensorLight in sensorLights)
            {
                sensorLight.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 255, 0, 255));
            }
           
        }
    }
}
