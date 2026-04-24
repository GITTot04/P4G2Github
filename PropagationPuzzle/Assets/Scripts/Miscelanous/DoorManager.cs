using UnityEngine;
using TMPro;
public class DoorManager : MonoBehaviour
{
    public static DoorManager instance;
    public int openDoorCount;
    public int MaxDoors;
    public GameObject minimapText;
    TextMeshProUGUI doorText;
    public SoundManager soundManager;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        doorText = minimapText.GetComponent<TextMeshProUGUI>();
    }
    public void FixedUpdate()
    {
        doorText.text = "Doors open: " + openDoorCount + " / " + MaxDoors;

        if (openDoorCount > MaxDoors)
        {
            doorText.faceColor = new Color32(255, 0, 0, 255);
            soundManager.doorSensor.doorCountExceeded = true;
        } else
        {
            doorText.faceColor = new Color32(0, 0, 0, 255);
            soundManager.doorSensor.doorCountExceeded = false;
        }
    }



}
