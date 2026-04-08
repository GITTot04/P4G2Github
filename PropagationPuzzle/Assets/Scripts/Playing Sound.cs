using UnityEngine;
using FMODUnity;

public class PlayingSound : MonoBehaviour
{
    public StudioEventEmitter emitter;
    void Start()
    {
        PlayerInput.instance.onAction += PlaySound;
    }

    void PlaySound()
    {
        emitter.Play();
    }
}
