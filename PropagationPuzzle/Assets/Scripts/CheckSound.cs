using UnityEngine;

public class CheckSound : MonoBehaviour
{
    public SoundRayStats rayStats;

    // debugging
    public bool showReflectionRays;

    int degreesOfRays = 360;
    Ray[] rayReflections;
    LayerMask playerMask;
    SoundRay[] bestRays;
    int newRayPosition;

    public float intensity;
    public float occlusion;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        SoundManager.instance.onPlaySound += FindOcclusionAndIntensity;
    }
    public void FindOcclusionAndIntensity()
    {
        ResetValues();
        SoundCheck();
        CalculateValues();
    }
    public void SoundCheck()
    {
        Ray ray = new Ray();
        rayReflections = new Ray[rayStats.MaxReflections];
        // Shoot rays once per degree for 360 degrees
        for (float angle = 0; angle < 360; angle += 360 / degreesOfRays)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            float xMove = Mathf.Cos(angleRad);
            float zMove = Mathf.Sin(angleRad);
            ray = new Ray(gameObject.transform.position, new Vector3(xMove, 0f, zMove).normalized); // Create the ray to be shot
            ShootReflectionRays(ray, 0, 0, 0);
        }
    }

    public void ShootReflectionRays(Ray ray, int priorReflections, int reflectionValue, int occlusion)
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
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, new Color(1 - (float)occlusion / 5, 1 - (float)occlusion / 5, 1, 1f - (float)reflectionIntensity / rayStats.MaxReflections));
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
                Physics.Raycast(hit.point + (gameObject.transform.position - hit.point) * -0.0001f, gameObject.transform.position - hit.point, out hit, Mathf.Infinity, ~playerMask);

                if (hit.collider.gameObject == gameObject) // Check if the player has direct LOS with the sound object
                {
                    SoundRay soundRay = new SoundRay((gameObject.transform.position - hit.point) * -1, reflectionIntensity, occlusion);
                    AddRay(soundRay);
                    break;
                }
                else
                {
                    for (int j = i; j > 0; j--) // Retrace the steps of the rays reflection until the player has LOS with the reflection point
                    {
                        Physics.Raycast(rayReflections[j].origin + (gameObject.transform.position - rayReflections[j].origin) * -0.0001f, gameObject.transform.position - rayReflections[j].origin, out hit, Mathf.Infinity, ~playerMask);


                        if (hit.collider.gameObject == gameObject)
                        {
                            SoundRay soundRay = new SoundRay((gameObject.transform.position - rayReflections[j].origin) * -1, reflectionIntensity, occlusion);
                            AddRay(soundRay);
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
        if (occlusion < rayStats.MaxOcclusions)
        {
            ShootReflectionRays(ray, reflection, reflectionValue, occlusion);
        }
    }

    int GetMostReflectRay()
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
    public void AddRay(SoundRay soundRay)
    {
        if (newRayPosition < bestRays.Length)
        {
            bestRays[newRayPosition] = soundRay;
            newRayPosition++;
        }
        else
        {
            int replaceRay = GetMostReflectRay();
            if (soundRay.reflections < bestRays[replaceRay].reflections)
            {
                bestRays[replaceRay] = soundRay;
            }
        }
    }
    public void ResetValues()
    {
        bestRays = new SoundRay[rayStats.BestRayCount];
        intensity = 0f;
        occlusion = 0f;
        newRayPosition = 0;

    }
    public void CalculateValues()
    {
        for (int i = 0; i < newRayPosition; i++)
        {

            //Calculate Ray Specific values
            float rayIntensity = 1f - Mathf.Clamp(((float)bestRays[i].reflections * 1f / (float)rayStats.MaxReflections), 0f, 1f);
            //Debug.Log(bestRays[i].reflections);
            float rayOcclusion = ((float)bestRays[i].occlusions / (float)rayStats.MaxOcclusions);
            //Adding to emitter
            intensity += rayIntensity;
            occlusion += rayOcclusion;
        }

        float averageOcclusion = 0;
        if (newRayPosition > 0)
        {
            averageOcclusion = occlusion / (float)(newRayPosition);
        }

        float finalIntensity = 0f;
        if (newRayPosition > 0)
        {
            finalIntensity = intensity / (float)(rayStats.BestRayCount);
        }

        // Add code to open door and play win sound if the sound reaches the sensor correctly
    }
}
