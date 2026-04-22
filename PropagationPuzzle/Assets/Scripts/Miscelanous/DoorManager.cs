using UnityEngine;
using TMPro;
public class DoorManager : MonoBehaviour
{
    public static DoorManager instance;
    public int openDoorCount;

    public GameObject minimapText;
    TextMeshProUGUI doorText;
    
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
        doorText.text = "Doors open: " + openDoorCount;
    }



}
