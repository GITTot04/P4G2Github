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
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += FindDoorSensor;
        }
    }
    void Start()
    {
        PlayerInput.instance.onAction += PlaySoundCode;
    }
    public void PlaySoundCode()
    {
        if (onPlaySound != null)
        {
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
}
