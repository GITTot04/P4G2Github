using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public delegate void PlaySound();
    public PlaySound onPlaySound;
    List<GameObject> activeAmplifiers = new List<GameObject>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
            CalculateAmplifiers();
            onPlaySound.Invoke();
        }
    }

    public void CalculateAmplifiers()
    {
        int maxOrder = 0;
        foreach (GameObject amp in activeAmplifiers) // Find the highest order
        {
            Amplifier ampScript = amp.GetComponent<Amplifier>();
            if (ampScript.order > maxOrder)
            {
                maxOrder = ampScript.order;
            }
        }
        for (int i = 0; i < maxOrder; i++) // Calculate values in the correct order
        {
            foreach (GameObject amp in activeAmplifiers)
            {
                Amplifier ampScript = amp.GetComponent<Amplifier>();
                if (ampScript.order == i)
                {
                    ampScript.FindOcclusionAndIntensity();
                }
            }
        }
    }
 
}
