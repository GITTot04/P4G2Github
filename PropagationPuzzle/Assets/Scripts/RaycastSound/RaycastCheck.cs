using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public SoundRayStats rayStats;

    // debugging
    public bool showReflectionRays;
    public bool showSoundDirectionRays;
    public bool showAverageSoundDirection;

    int degreesOfRays = 360;
    public float occlusionForFmod;
    int successfulRays;
    Ray[] rayReflections;
    SoundRay[] soundDirectionsAndReflections;
    LayerMask playerMask;
    LayerMask doorMask;

    //Oskar
    public SpacialSoundInterpreter soundInterpreter;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        doorMask = LayerMask.GetMask("Door");
    }
    private void FixedUpdate()
    {
        //Oskar 
        soundInterpreter.ResetEmitterValues();

        SoundCheck();

        soundInterpreter.SetEmitterValues();
        
    }
    public void SoundCheck()
    {
        Ray ray = new Ray();
        rayReflections = new Ray[rayStats.MaxReflections];
        soundDirectionsAndReflections = new SoundRay[degreesOfRays * rayStats.MaxOcclusions]; // 360 degrees times the maximum amount of occlusions won't exceed the array
        successfulRays = 0;
        // Shoot rays once per degree for 360 degrees
        for (float angle = 0; angle < 360; angle += 360/degreesOfRays)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            float xMove = Mathf.Cos(angleRad);
            float zMove = Mathf.Sin(angleRad);
            ray = new Ray(gameObject.transform.position, new Vector3(xMove, 0f, zMove).normalized); // Create the ray to be shot
            ShootReflectionRays(ray, 0, 0, 0, false);
        }

        float soundXValue = 0;
        float soundZValue = 0;
        float totalReflection = 0; //idk if this is needed
        float totalOcclusion = 0; // idk if this is needed
        for (int i = 0; i < successfulRays; i++) // Calculate total values
        {
            soundXValue += soundDirectionsAndReflections[i].direction.x * (1 - soundDirectionsAndReflections[i].reflections / rayStats.MaxReflections);
            soundZValue += soundDirectionsAndReflections[i].direction.z * (1 - soundDirectionsAndReflections[i].reflections / rayStats.MaxReflections);
            totalReflection += soundDirectionsAndReflections[i].reflections; // idk if this is needed
            totalOcclusion += soundDirectionsAndReflections[i].occlusions; // idk if this is needed
        }
        if (successfulRays > 0) // Get the averages
        {
            float averageXValue = soundXValue / successfulRays;
            float averageZValue = soundZValue / successfulRays;
            float averageReflection = totalReflection / successfulRays; //idk if this is needed
            float averageOcclusion = totalOcclusion / successfulRays; // idk if this is needed
            
            //debugging
            if (showAverageSoundDirection)
            {
                Debug.DrawRay(gameObject.transform.position, new Vector3(averageXValue, 0, averageZValue), new Color(0, 0, 0, 1));
            }
        }
    }

    public void ShootReflectionRays(Ray ray, int priorReflections, int reflectionValue, int occlusion, bool calledByOccludedRay)
    {
        int reflectionIntensity = reflectionValue;
        RaycastHit hit;
        for (int i = priorReflections; i < rayStats.MaxReflections && reflectionIntensity < rayStats.MaxReflections; i++)
        {
            rayReflections[i] = ray;
            Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerMask); // Shoots the initial ray ignoring the player

            // debugging
            if (showReflectionRays)
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, new Color(1 - (float)occlusion/5, 1 - (float)occlusion/5, 1, 1f - (float)reflectionIntensity / rayStats.MaxReflections));
            }

            if (hit.collider.gameObject.tag == "Door") // Call the method for shooting the occluded ray when a door is hit
            {
                if (reflectionIntensity * 2 > rayStats.MaxReflections)
                {
                    break;
                }
                else
                {
                    ShootOccludedRay(new Ray(hit.point + (ray.direction.normalized * 0.0001f), ray.direction.normalized), i, reflectionIntensity * 2, occlusion);
                    reflectionIntensity *= 2;
                }
            }

            if (hit.collider.gameObject.tag == "Speaker")
            {
                /*
                if (calledByOccludedRay) // Rays that have already passed through doors should not get blocked by the doors on the way back
                {
                    Physics.Raycast(hit.point + (gameObject.transform.position - hit.point) * -0.0001f, gameObject.transform.position - hit.point, out hit, Mathf.Infinity, ~doorMask);
                }
                else
                {
                    Physics.Raycast(hit.point + (gameObject.transform.position - hit.point) * -0.0001f, gameObject.transform.position - hit.point, out hit, Mathf.Infinity);
                }
                */
                //idk which is more correct (above or below)
                Physics.Raycast(hit.point + (gameObject.transform.position - hit.point) * -0.0001f, gameObject.transform.position - hit.point, out hit, Mathf.Infinity);

                if (hit.collider.gameObject.tag == "Player") // Check if the player has direct LOS with the sound object
                {
                    soundDirectionsAndReflections[successfulRays] = new SoundRay((gameObject.transform.position - hit.point) * -1, reflectionIntensity, occlusion);
                    successfulRays++;
                    break;
                }
                else
                {
                    for (int j = i; j > 0; j--) // Retrace the steps of the rays reflection until the player has LOS with the reflection point
                    {
                        /*
                        if (calledByOccludedRay)
                        {
                            Physics.Raycast(rayReflections[j].origin + (gameObject.transform.position - rayReflections[j].origin) * -0.0001f, gameObject.transform.position - rayReflections[j].origin, out hit, Mathf.Infinity, ~doorMask);
                        }
                        else
                        {
                            Physics.Raycast(rayReflections[j].origin + (gameObject.transform.position - rayReflections[j].origin) * -0.0001f, gameObject.transform.position - rayReflections[j].origin, out hit, Mathf.Infinity);
                        }
                        */
                        //idk which is more correct (above or below)
                        Physics.Raycast(rayReflections[j].origin + (gameObject.transform.position - rayReflections[j].origin) * -0.0001f, gameObject.transform.position - rayReflections[j].origin, out hit, Mathf.Infinity);


                        if (hit.collider.gameObject.tag == "Player")
                        {
                            SoundRay soundRay = new SoundRay((gameObject.transform.position - rayReflections[j].origin) * -1, reflectionIntensity, occlusion);
                            soundDirectionsAndReflections[successfulRays] = soundRay;
                            successfulRays++;

                            //Oskar
                            soundInterpreter.AddSoundRay(soundRay);

                            // debugging
                            if (showSoundDirectionRays)
                            {
                                Debug.DrawRay(rayReflections[j].origin, gameObject.transform.position - rayReflections[j].origin, new Color(1, 0.75f, 0.75f, 1));
                            }

                            break;
                        }
                    }
                }
                break;
            }
            else // Find the reflected ray
            {
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
            reflectionIntensity++;
        }
    }

    public void ShootOccludedRay(Ray ray, int reflection, int reflectionValue, int occlusion) // Increase occlusion and shoot out an occluded ray. May call itself a few times
    {
        occlusion += 1;
        occlusionForFmod = occlusion; // Set the occlusion value for FMOD
        if (occlusion < rayStats.MaxOcclusions)
        {
            ShootReflectionRays(ray, reflection, reflectionValue, occlusion, true);
        }
    }
}
