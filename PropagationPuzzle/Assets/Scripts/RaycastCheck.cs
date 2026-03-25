using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastCheck : MonoBehaviour
{
    //For debugging
    public bool showReflectionRays;
    public bool showSoundDirectionRays;
    public bool showFirstReflectionRays;
    // ^^
    int maxReflections = 10;
    int playerLayer = 6;
    private void FixedUpdate()
    {
        OnCast();
    }
    public void OnCast()
    {
        Ray ray = new Ray();
        Ray[] rayReflections = new Ray[maxReflections];
        for (float angle = 0; angle < 360; angle += 1)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            float xMove = Mathf.Cos(angleRad);
            float zMove = Mathf.Sin(angleRad);
            ray = new Ray(gameObject.transform.position, new Vector3(xMove, 0f, zMove).normalized);
            Ray originalRay = ray;
            float originalLength = 0;
            RaycastHit hit;
            for (int i = 0; i < maxReflections; i++)
            {
                rayReflections[i] = ray;
                Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerLayer);
                if (i == 0)
                {
                    originalLength = hit.distance;
                }
                // For debugging
                if (showReflectionRays)
                {
                    float drawLength = hit.distance;
                    Debug.DrawRay(ray.origin, ray.direction * drawLength, new Color(1, 1, 1, 0.5f - 0.5f * (float)i / maxReflections));
                }
                if (hit.collider.gameObject.tag == "Speaker")
                {
                    for (int j = i; j > 0; j--)
                    {
                        Physics.Raycast(rayReflections[j].origin, gameObject.transform.position - rayReflections[j].origin, out hit, Mathf.Infinity);
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            if (showSoundDirectionRays)
                            {
                                Debug.DrawRay(rayReflections[j].origin, gameObject.transform.position - rayReflections[j].origin /** hit.distance*/, new Color(1, 0.5f, 0.5f, 1));
                            }
                            break;
                        }
                    }
                    // For debugging         FIRST REFLECTION
                    if (showFirstReflectionRays)
                    {
                        Debug.DrawRay(originalRay.origin, originalRay.direction * originalLength, new Color(1, 0.5f, 0.5f, 1/* - (float)i / maxReflections*/));
                    }
                    break;
                }
                else
                {
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                }
            }
        }
    }
}
