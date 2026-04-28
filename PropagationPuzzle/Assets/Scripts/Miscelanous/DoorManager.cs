using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class DoorManager : MonoBehaviour
{
    public static DoorManager instance;
    public int openDoorCount;
    public int MaxDoors;
    public GameObject minimapText;
    TextMeshProUGUI doorText;
    public SoundManager soundManager;
    public bool hasWon;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += SetAllowedDoors;
            SceneManager.sceneLoaded += ChangeWinStatus;
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

    void SetAllowedDoors(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Level1":
                MaxDoors = 1;
                break;
            case "Level2":
                MaxDoors = 1;
                break;
            default:
                break;
        }
    }
    void ChangeWinStatus(Scene scene, LoadSceneMode mode)
    {
        hasWon = false;
    }
}
