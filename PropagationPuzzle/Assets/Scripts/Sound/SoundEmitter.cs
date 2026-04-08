using UnityEngine;

public class SoundEmitter : MonoBehaviour
{
    public SoundRayStats rayStats;
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
        Debug.Log(soundRay.occlusions);
        //Calculation Ray Specific values
        float rayIntensity = (float)1 - ((float)soundRay.reflections / (float)rayStats.MaxReflections);
        float rayOcclusion = (float)1 - ((float)soundRay.occlusions / (float)rayStats.MaxOcclusions);

        //Adding to emitter
        intensity += rayIntensity;
        occlusion += rayOcclusion;
    }
}
