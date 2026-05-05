using UnityEngine;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.UI;
using UnityEngine.UI;


public class DoorSensor : CheckSound
{
    public float maximumOcclusion = 1;
    public float minimumIntensity = 0;
    public ExitDoor[] exitDoors = new ExitDoor[2];
    public List<GameObject> sensorLights = new List<GameObject>();
    public Image fillBar;

    public bool doorCountExceeded = false;
    public override void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        (float, float) calculate = CalculateValues();
        if (calculate.Item2 > minimumIntensity) // (occlusion,intensity) is returned from this method.
        {
            fillBar.fillAmount = Mathf.Lerp(0.1f, 1, calculate.Item1);
            foreach (GameObject sensorLight in sensorLights)
            {
                sensorLight.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 255, 0, 255));
            }

            if (!doorCountExceeded && !DoorManager.instance.hasWon && calculate.Item1 < maximumOcclusion) {
                foreach (ExitDoor exitDoor in exitDoors)
                {
                    exitDoor.UnlockDoor();
                }
                DoorManager.instance.hasWon = true;
            }
        }
        else
        {
            fillBar.fillAmount = 0f;
        }

       
    }
}
