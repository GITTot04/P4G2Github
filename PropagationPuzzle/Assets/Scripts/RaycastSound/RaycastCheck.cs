using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    public SoundRayStats rayStats;

    // debugging
    public bool showReflectionRays;
    public bool showSoundDirectionRays;
    public bool showAverageSoundDirection;

    int degreesOfRays = 360;
    int successfulRays;
    Ray[] rayReflections;
    SoundRay[] soundDirectionsAndReflections;
    LayerMask playerMask;
    bool addAmplifierOcclusion;
    float amplifierOcclusion;

    //Oskar
    public SpacialSoundInterpreter soundInterpreter;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
    }
    private void FixedUpdate()
    {
        //Oskar 
        soundInterpreter.ResetEmitterValues();

        RaycastCheckPool.instance.ReturnAllSoundRays();

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
            ShootReflectionRays(ray, 0, 0, 0);
        }

        float soundXValue = 0;
        float soundZValue = 0;
        for (int i = 0; i < successfulRays; i++) // Calculate total values
        {
            soundXValue += soundDirectionsAndReflections[i].direction.x * (1 - soundDirectionsAndReflections[i].reflections / rayStats.MaxReflections);
            soundZValue += soundDirectionsAndReflections[i].direction.z * (1 - soundDirectionsAndReflections[i].reflections / rayStats.MaxReflections);
        }
        if (successfulRays > 0) // Get the averages
        {
            float averageXValue = soundXValue / successfulRays;
            float averageZValue = soundZValue / successfulRays;
            
            //debugging
            if (showAverageSoundDirection)
            {
                Debug.DrawRay(gameObject.transform.position, new Vector3(averageXValue, 0, averageZValue), new Color(0, 0, 0, 1));
            }
        }
    }

    public void ShootReflectionRays(Ray ray, int priorReflections, int reflectionValue, float occlusion)
    {
        int reflectionIntensity = reflectionValue;
        RaycastHit hit;
        for (int i = priorReflections; i < rayStats.MaxReflections && reflectionIntensity < rayStats.MaxReflections; i++)
        {
            rayReflections[i] = ray;
            Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerMask); // Shoots the initial ray ignoring the player
            if (hit.collider == null) 
            {
                return;
            }
            // debugging
            if (showReflectionRays)
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, new Color(1f - occlusion/5f, 1f - occlusion/5f, 1f, 1f - (float)reflectionIntensity / rayStats.MaxReflections));
            }

            if (hit.collider.gameObject.tag == "Door") // Call the method for shooting the occluded ray when a door is hit
            {
                if (reflectionIntensity + 2 > rayStats.MaxReflections)
                {
                    break;
                }
                else
                {
                    ShootOccludedRay(new Ray(hit.point + (ray.direction.normalized * 0.0001f), ray.direction.normalized), i, reflectionIntensity + 2, occlusion);
                    reflectionIntensity += 2;
                }
            }

            if (hit.collider.gameObject.tag == "Speaker" || hit.collider.gameObject.tag == "Amplifier")
            {
                if (hit.collider.gameObject.tag == "Amplifier")
                {
                    if (hit.collider.gameObject.GetComponent<Amplifier>().isAmplifying)
                    {
                        addAmplifierOcclusion = true;
                        amplifierOcclusion = hit.collider.gameObject.GetComponent<Amplifier>().amplifierOcclusion;
                    }
                    else
                    {
                        addAmplifierOcclusion = false;
                        if (reflectionIntensity + 1 > rayStats.MaxReflections)
                        {
                            break;
                        }
                        else
                        {
                            ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                            ShootReflectionRays(ray, i + 1, reflectionIntensity + 1, occlusion);
                            break;
                        }
                    }
                }
                Physics.Raycast(hit.point + (gameObject.transform.position - hit.point) * -0.0001f, gameObject.transform.position - hit.point, out hit, Mathf.Infinity);

                if (hit.collider.gameObject.tag == "Player") // Check if the player has direct LOS with the sound object
                {
                    if (addAmplifierOcclusion)
                    {
                        SoundRay soundRay = RaycastCheckPool.instance.GetSoundRay((gameObject.transform.position - hit.point) * -1, reflectionIntensity, occlusion + amplifierOcclusion);
                        soundDirectionsAndReflections[successfulRays] = soundRay;
                        successfulRays++;
                        soundInterpreter.AddSoundRay(soundRay);
                    }
                    else
                    {
                        SoundRay soundRay = RaycastCheckPool.instance.GetSoundRay((gameObject.transform.position - hit.point) * -1, reflectionIntensity, occlusion);
                        soundDirectionsAndReflections[successfulRays] = soundRay;
                        successfulRays++;
                        soundInterpreter.AddSoundRay(soundRay);
                    }

                    break;
                }
                else
                {
                    for (int j = i; j > 0; j--) // Retrace the steps of the rays reflection until the player has LOS with the reflection point
                    {
                        Physics.Raycast(rayReflections[j].origin + (gameObject.transform.position - rayReflections[j].origin) * -0.0001f, gameObject.transform.position - rayReflections[j].origin, out hit, Mathf.Infinity);


                        if (hit.collider.gameObject.tag == "Player")
                        {
                            if (addAmplifierOcclusion)
                            {
                                SoundRay soundRay = RaycastCheckPool.instance.GetSoundRay((gameObject.transform.position - rayReflections[j].origin) * -1, reflectionIntensity, occlusion + amplifierOcclusion);
                                soundDirectionsAndReflections[successfulRays] = soundRay;
                                successfulRays++;
                                soundInterpreter.AddSoundRay(soundRay);
                            }
                            else
                            {
                                SoundRay soundRay = RaycastCheckPool.instance.GetSoundRay((gameObject.transform.position - rayReflections[j].origin) * -1, reflectionIntensity, occlusion);
                                soundDirectionsAndReflections[successfulRays] = soundRay;
                                successfulRays++;
                                soundInterpreter.AddSoundRay(soundRay);
                            }

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

    public void ShootOccludedRay(Ray ray, int reflection, int reflectionValue, float occlusion) // Increase occlusion and shoot out an occluded ray. May call itself a few times
    {
        occlusion += 1f;
        if (occlusion < rayStats.MaxOcclusions)
        {
            ShootReflectionRays(ray, reflection, reflectionValue, occlusion);
        }
    }
}
