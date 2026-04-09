using UnityEngine;
using FMODUnity;

public class SoundEmitter : MonoBehaviour
{
    public StudioEventEmitter eventEmitter;
    public SoundRayStats rayStats;
    public float intensityMulitplier;

    public float intensity;
    public float occlusion;
    public int rayCount;

    void Start()
    {
        SoundManager.instance.onPlaySound += PlaySound;
    }
    public void ResetValues ()
    {
        intensity = 0f;
        occlusion = 0f;
        rayCount = 0;
        
    }
    public void AddRay (SoundRay soundRay)
    {
        //Calculation Ray Specific values
        float rayIntensity = (float)1 - Mathf.Clamp(((float)soundRay.reflections * 2 / (float)rayStats.MaxReflections),0f,1f);
        float rayOcclusion = ((float)soundRay.occlusions / (float)rayStats.MaxOcclusions);

        //Adding to emitter
        intensity += rayIntensity;
        occlusion += rayOcclusion;
        //Debug.Log(soundRay.occlusions);

        rayCount += 1;
    }
    public void SetValues ()
    {
        //Occlusion
        float averageOcclusion = 0;
        if (rayCount > 0)
        {
           
            averageOcclusion = occlusion / (float)(rayCount);
            
        } 
        float finalOcclusion = Mathf.Lerp (0f, rayStats.OcclusionCap, averageOcclusion);
        if (rayCount > 10)
        {
           Debug.Log("Occlusion: " +  occlusion + ", average occlusion:" + averageOcclusion + ", raycount:" + rayCount);
        }
        //eventEmitter.occlusionIntensity = finalOcclusion;
        //eventEmitter.EventInstance.setParameterByName("Occlusion", 5);
        //eventEmitter.EventInstance.setParameterByName("Occlusion", 10);

        //Intensity
        /*if (intensity > rayStats.MaxIntensity)
        {
            intensity = rayStats.MaxIntensity;
        }*/
        float finalIntensity = intensity / rayCount;
        eventEmitter.EventInstance.setVolume(finalIntensity);
    }
    void PlaySound ()
    {
        eventEmitter.Play();
    }
}
