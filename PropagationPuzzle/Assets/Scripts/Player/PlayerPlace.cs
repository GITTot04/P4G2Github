using UnityEngine;

public class PlayerPlace : MonoBehaviour
{
    public GameObject amplifierPrefab;
    public bool amp1Placed;
    public bool amp2Placed;
    public bool amp3Placed;
    void Start()
    {
        PlayerInput.instance.onPlace += ChooseAmplifier;
    }
    public void ChooseAmplifier(string button)
    {
        switch (button)
        {
            case "1":
                break;
            case "2":
                break;
            case "3":
                break;
            default:
                break;
        }
    }
    public void RemoveAmplifier()
    {

    }
    public void PlaceAmplifier()
    {

    }
}
