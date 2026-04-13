using UnityEngine;
using FMODUnity;

public class SoundEmitter : MonoBehaviour
{
    public StudioEventEmitter eventEmitter;
    public SoundRayStats rayStats;
    public float intensityMulitplier;

    public float intensity;
    float lastIntensity;
    public float occlusion;

    

    SoundRay[] bestRays;
    int newRayPosition;

    void Start()
    {
        bestRays = new SoundRay[rayStats.BestRayCount];
        SoundManager.instance.onPlaySound += PlaySound;
        ResetValues();
    }
    public void ResetValues ()
    {
        intensity = 0f;
        occlusion = 0f;

        newRayPosition = 0;
        
    }
    SoundRay GetMostReflectRay ()
    {
        SoundRay mostReflectRay = bestRays[0];

        for (int i = 1; i < bestRays.Length; i++)
        {
            if (bestRays[i].reflections > mostReflectRay.reflections)
            {
                mostReflectRay = bestRays[i];
            }
        }
        return mostReflectRay;
    }
    public void AddRay (SoundRay soundRay)
    {
        if (bestRays == null)
        {
            bestRays = new SoundRay[rayStats.BestRayCount];
            //Debug.Log("Bestrays was null");
        }
        
        if (newRayPosition < bestRays.Length)
        {
            bestRays[newRayPosition] = soundRay;
            newRayPosition++;
        } else
        {
            SoundRay replaceRay = GetMostReflectRay();
            if (soundRay.reflections < replaceRay.reflections)
            {
                replaceRay = soundRay;
            }
        }

        /*

        //Calculate Ray Specific values
        float rayIntensity = (float)1 - Mathf.Clamp(((float)soundRay.reflections * 2 / (float)rayStats.MaxReflections),0f,1f);
        float rayOcclusion = ((float)soundRay.occlusions / (float)rayStats.MaxOcclusions);

        //Adding to emitter
        intensity += rayIntensity;
        occlusion += rayOcclusion;
        //Debug.Log(soundRay.occlusions);

        rayCount += 1;
        */
    }
    public void SetValues ()
    {
        for (int i = 0; i < newRayPosition; i++)
        {

            //Calculate Ray Specific values
            float rayIntensity = (float)1 - Mathf.Clamp(((float)bestRays[i].reflections * 1.1f / (float)rayStats.MaxReflections), 0f, 1f);
            float rayOcclusion = ((float)bestRays[i].occlusions / (float)rayStats.MaxOcclusions);
            
            //Adding to emitter
            intensity += rayIntensity;
            occlusion += rayOcclusion;
            //Debug.Log(soundRay.occlusions);
        }

        //Occlusion
        float averageOcclusion = 0;
        if (newRayPosition > 0)
        {
           
            averageOcclusion = occlusion / (float)(newRayPosition);
            
        }
        float finalOcclusion = Mathf.Lerp (0f, rayStats.OcclusionCap, averageOcclusion);
        
        /*if (rayCount > 10)
        {
           Debug.Log("Occlusion: " +  occlusion + ", average occlusion:" + averageOcclusion + ", raycount:" + rayCount);
        }*/
        eventEmitter.occlusionIntensity = finalOcclusion;
        //eventEmitter.EventInstance.setParameterByName("Occlusion", 5);
        //eventEmitter.EventInstance.setParameterByName("Occlusion", 10);

        //Intensity
        /*if (intensity > rayStats.MaxIntensity)
        {
            intensity = rayStats.MaxIntensity;
        }*/
        float finalIntensity = 0f;
        if (newRayPosition > 0)
        {
            finalIntensity = intensity / (float)(rayStats.BestRayCount);
        }

        float intensitySet = Mathf.Lerp(lastIntensity, finalIntensity, Time.fixedDeltaTime * rayStats.IntensityChangeSpeed);
        eventEmitter.EventInstance.setVolume(intensitySet);
        lastIntensity = intensitySet;
    }
    void PlaySound ()
    {
        eventEmitter.Play();
    }
}
