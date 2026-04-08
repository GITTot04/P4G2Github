using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    // debugging
    public bool showReflectionRays;
    public bool showSoundDirectionRays;
    public bool showAverageSoundDirection;

    public float occlusionForFmod;
    int maxReflections = 10;
    int maxOcclusions = 5;
    int successfulRays;
    Ray[] rayReflections;
    SoundRay[] soundDirectionsAndReflections;
    LayerMask playerMask;
    LayerMask doorMask;
    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        doorMask = LayerMask.GetMask("Door");
    }
    private void FixedUpdate()
    {
        SoundCheck();
    }
    public void SoundCheck()
    {
        Ray ray = new Ray();
        rayReflections = new Ray[maxReflections];
        soundDirectionsAndReflections = new SoundRay[360 * maxOcclusions]; // 360 degrees times the maximum amount of occlusions won't exceed the array
        successfulRays = 0;
        // Shoot rays once per degree for 360 degrees
        for (float angle = 0; angle < 360; angle += 1)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            float xMove = Mathf.Cos(angleRad);
            float zMove = Mathf.Sin(angleRad);
            ray = new Ray(gameObject.transform.position, new Vector3(xMove, 0f, zMove).normalized); // Create the ray to be shot
            ShootReflectionRays(ray, 0, 0, false);
        }

        float soundXValue = 0;
        float soundZValue = 0;
        float totalReflection = 0; //idk if this is needed
        float totalOcclusion = 0; // idk if this is needed
        for (int i = 0; i < successfulRays; i++) // Calculate total values
        {
            soundXValue += soundDirectionsAndReflections[i].direction.x * (1 - soundDirectionsAndReflections[i].reflections / maxReflections);
            soundZValue += soundDirectionsAndReflections[i].direction.z * (1 - soundDirectionsAndReflections[i].reflections / maxReflections);
            totalReflection += soundDirectionsAndReflections[i].reflections; // idk if this is needed
            totalOcclusion += soundDirectionsAndReflections[i].occlusions; // idk if this is needed
        }
        if (successfulRays > 0) // Get the averages
        {
            float averageXValue = soundXValue / successfulRays;
            float averageZValue = soundZValue / successfulRays;
            float averageReflection = totalReflection / successfulRays; //idk if this is needed
            float averageOcclusion = totalOcclusion / successfulRays; // idk if this is needed
            occlusionForFmod = averageOcclusion; // Set the occlusion value for FMOD

            //debugging
            if (showAverageSoundDirection)
            {
                Debug.DrawRay(gameObject.transform.position, new Vector3(averageXValue, 0, averageZValue), new Color(0, 0, 0, 1));
            }
        }
    }

    public void ShootReflectionRays(Ray ray, int priorReflection, int occlusion, bool calledByOccludedRay)
    {
        RaycastHit hit;
        for (int i = priorReflection; i < maxReflections; i++)
        {
            rayReflections[i] = ray;
            Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerMask); // Shoots the initial ray ignoring the player

            // debugging
            if (showReflectionRays)
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, new Color(1 - (float)occlusion/5, 1 - (float)occlusion/5, 1, 1f - (float)i / maxReflections));
            }

            if (hit.collider.gameObject.tag == "Door") // Call the method for shooting the occluded ray when a door is hit
            {
                ShootOccludedRay(new Ray(hit.point + (ray.direction.normalized * 0.0001f), ray.direction.normalized), i, occlusion);
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
                    soundDirectionsAndReflections[successfulRays] = new SoundRay((gameObject.transform.position - hit.point) * -1, i, occlusion);
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
                            soundDirectionsAndReflections[successfulRays] = new SoundRay((gameObject.transform.position - rayReflections[j].origin) * -1, i, occlusion);
                            successfulRays++;

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
        }
    }

    public void ShootOccludedRay(Ray ray, int reflection, int occlusion) // Increase occlusion and shoot out an occluded ray. May call itself a few times
    {
        occlusion += 1;
        if (occlusion < maxOcclusions)
        {
            ShootReflectionRays(ray, reflection, occlusion, true);
        }
    }
}
