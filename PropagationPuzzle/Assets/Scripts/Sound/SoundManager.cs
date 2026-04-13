using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public delegate void PlaySound();
    public PlaySound onPlaySound;
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
            onPlaySound.Invoke();
        }
    }
 
}
