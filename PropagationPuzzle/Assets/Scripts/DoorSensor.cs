using UnityEngine;
using System.Collections.Generic;

public class DoorSensor : CheckSound
{
    public float maximumOcclusion = 1;
    public float minimumIntensity = 0;
    public ExitDoor[] exitDoors = new ExitDoor[2];
    public List<GameObject> sensorLights = new List<GameObject>();
    public override void FindOcclusionAndIntensity()
    {
        Debug.Log("Called");

        ResetValues();
        SoundCheck();
        (float, float) calculate = CalculateValues();
        if (calculate.Item1 < maximumOcclusion && calculate.Item2 > minimumIntensity) // (occlusion,intensity) is returned from this method.
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
