using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SoundEmitter : MonoBehaviour
{
    public StudioEventEmitter eventEmitter;
    public SoundRayStats rayStats;
    public float intensityMulitplier;

    public float intensity;
    float lastIntensity;
    public float occlusion;
    public bool debugReflections;

    SoundRay[] bestRays;
    int newRayPosition;

    float intensitySet = 0f;
    float finalOcclusion = 0;

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
    int GetMostReflectRay ()
    {
        int arrayPos;
        SoundRay mostReflectRay = bestRays[0];
        arrayPos = 0;

        for (int i = 1; i < bestRays.Length; i++)
        {
            if (bestRays[i].reflections > mostReflectRay.reflections)
            {
                mostReflectRay = bestRays[i];
                arrayPos = i;
            }
        }
        return arrayPos;
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
            int replaceRay = GetMostReflectRay();
            if (soundRay.reflections < bestRays[replaceRay].reflections)
            {
                bestRays[replaceRay] = soundRay;
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
            float rayIntensity = (float)1 - Mathf.Clamp(((float)bestRays[i].reflections * 1f / (float)rayStats.MaxReflections), 0f, 1f);
            if (debugReflections)
            {
                Debug.Log(bestRays[i].reflections);
            }
            float rayOcclusion = ((float)bestRays[i].occlusions / (float)rayStats.MaxOcclusions);
            //Adding to emitter
            intensity += rayIntensity;
            occlusion += rayOcclusion;
        }

        //Occlusion
        if (newRayPosition > 0)
        {
            float averageOcclusion = occlusion / (float)(newRayPosition);
            
            finalOcclusion = Mathf.Lerp(0f, rayStats.OcclusionCap, averageOcclusion);

            eventEmitter.occlusionIntensity = finalOcclusion;
        }
      
        /*if (rayCount > 10)
        {
           Debug.Log("Occlusion: " +  occlusion + ", average occlusion:" + averageOcclusion + ", raycount:" + rayCount);
        }*/
        
    
        float finalIntensity = 0f;
        if (newRayPosition > 0)
        {
            finalIntensity = intensity / (float)(rayStats.BestRayCount);
        }


        intensitySet = Mathf.Lerp(lastIntensity, finalIntensity, Time.fixedDeltaTime * rayStats.IntensityChangeSpeed);

        eventEmitter.EventInstance.setVolume(intensitySet);
        lastIntensity = intensitySet;
        //Debug.Log(intensitySet);
    }
    void PlaySound ()
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventEmitter.EventReference);
        RuntimeManager.AttachInstanceToGameObject(eventInstance, gameObject);
        eventInstance.setParameterByName("Occlusion", finalOcclusion);
        eventInstance.setVolume(intensitySet);
        eventInstance.start();
        eventInstance.release();
    }

}
