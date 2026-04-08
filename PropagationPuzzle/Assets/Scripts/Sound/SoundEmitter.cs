using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public float intensityMulitplier;

    public float intensity;
    public float occlusion;
    public void ResetValues ()
    {
        intensity = 0f;
        occlusion = 0f;
    }
    public void AddRay (SoundRay soundRay)
    {
        //float rayIntensity = soundRay.reflections
        //SOMETHING
    }
}
