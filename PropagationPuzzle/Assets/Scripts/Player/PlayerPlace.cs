using UnityEngine;
using System.Collections;

public class PlayerPlace : MonoBehaviour
{
    public GameObject amplifierPrefab;
    public bool amp1Placed;
    public bool amp2Placed;
    public bool amp3Placed;
    public GameObject playerHead;
    void Start()
    {
        PlayerInput.instance.onPlace += ChooseAmplifier; // might need to be in onEnable and onDisable when multiple levels are added
    }
    public void ChooseAmplifier(string button)
    {
        if (int.Parse(button) <= SoundManager.instance.allowedAmplifiers)
        {
            switch (button)
            {
                case "1":
                    if (amp1Placed)
                    {
                        StartCoroutine(RemoveAmplifier(button));
                    }
                    else
                    {
                        PlaceAmplifier(button);
                    }
                    break;
                case "2":
                    if (amp2Placed)
                    {
                        StartCoroutine(RemoveAmplifier(button));
                    }
                    else
                    {
                        PlaceAmplifier(button);
                    }
                    break;
                case "3":
                    if (amp3Placed)
                    {
                        StartCoroutine(RemoveAmplifier(button));
                    }
                    else
                    {
                        PlaceAmplifier(button);
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public IEnumerator RemoveAmplifier(string ampNumber)
    {
        foreach (Amplifier amp in SoundManager.instance.activeAmplifiers)
        {
            if (amp.order == int.Parse(ampNumber) - 1)
            {
                while (!SoundManager.instance.canDeleteAmp) // Makes sure you can't delete an amplifier that might get called when calculating sound
                {
                    yield return null;
                }
                Destroy(amp.gameObject);
                break;
            }
        }
        switch (ampNumber)
        {
            case "1":
                amp1Placed = false;
                break;
            case "2":
                amp2Placed = false;
                break;
            case "3":
                amp3Placed = false;
                break;
            default:
                break;
        }
        yield return null;
    }
    public void PlaceAmplifier(string ampNumber)
    {
        RaycastHit hit;
        Ray ray = new Ray(gameObject.transform.position, playerHead.transform.forward); // Create the ray to be shot
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        if (hit.collider.gameObject.tag == "Wall")
        {
            GameObject amp = Instantiate(amplifierPrefab, hit.point + 0.00001f * hit.normal, Quaternion.LookRotation(hit.normal,hit.transform.up)/*new Quaternion(0,0,0,0)*/);
            amp.transform.Rotate(0f, -90f, 0f);
            amp.GetComponent<Amplifier>().order = int.Parse(ampNumber) - 1;
            switch (ampNumber)
            {
                case "1":
                    amp1Placed = true;
                    break;
                case "2":
                    amp2Placed = true;
                    break;
                case "3":
                    amp3Placed = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.Log("Can't place here");
            // error message
        }
    }
}
