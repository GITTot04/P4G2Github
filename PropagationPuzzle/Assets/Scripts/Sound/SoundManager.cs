using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public delegate void PlaySound();
    public PlaySound onPlaySound;
    public List<Amplifier> activeAmplifiers = new List<Amplifier>();
    public DoorSensor doorSensor;
    public bool canDeleteAmp = true;
    bool canPlaySound = true;
    float playSoundCooldown = 1.5f;
    public int allowedAmplifiers;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += FindDoorSensor;
            SceneManager.sceneLoaded += SetAllowedAmplifiers;
        }
    }
    void Start()
    {
        PlayerInput.instance.onAction += PlaySoundCode;
    }
    public void PlaySoundCode()
    {
        if (onPlaySound != null && canPlaySound)
        {
            StartCoroutine(PlaySoundCooldown());
            canDeleteAmp = false;
            CalculateAmplifiers();
            doorSensor.FindOcclusionAndIntensity();
            onPlaySound.Invoke();
            canDeleteAmp = true;
        }
    }

    public void CalculateAmplifiers()
    {
        if (activeAmplifiers.Count > 0)
        {
            int maxOrder = 0;
            foreach (Amplifier amp in activeAmplifiers) // Find the highest order
            {
                amp.ResetValues();
                amp.amplifierOcclusion = 0;
                if (amp.order > maxOrder)
                {
                    maxOrder = amp.order;
                }
            }
            for (int i = 0; i <= maxOrder; i++) // Calculate values in the correct order
            {
                foreach (Amplifier amp in activeAmplifiers)
                {
                    if (amp.order == i)
                    {
                        amp.FindOcclusionAndIntensity();
                    }
                }
            }
        }
    }

    public void FindDoorSensor(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.Find("DoorSensor") != null)
        {
            doorSensor = GameObject.Find("DoorSensor").GetComponent<DoorSensor>();
        }
    }

    IEnumerator PlaySoundCooldown()
    {
        canPlaySound = false;
        yield return new WaitForSeconds(playSoundCooldown);
        canPlaySound = true;
    }

    void SetAllowedAmplifiers(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Level1":
                allowedAmplifiers = 0;
                break;
            case "Level2":
                allowedAmplifiers = 0;
                break;
            default:
                break;
        }
    }
}
