using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastCheck : MonoBehaviour
{
    //For debugging
    public bool showReflectionRays;
    public bool showSoundDirectionRays;
    // ^^
    int maxReflections = 10;
    LayerMask playerMask;

    private void Start()
    {
        playerMask = LayerMask.GetMask("Player");
    }
    private void FixedUpdate()
    {
        SoundCheck();
    }
    public void SoundCheck()
    {
        Ray ray = new Ray();
        Ray[] rayReflections = new Ray[maxReflections];
        for (float angle = 0; angle < 360; angle += 1)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            float xMove = Mathf.Cos(angleRad);
            float zMove = Mathf.Sin(angleRad);
            ray = new Ray(gameObject.transform.position, new Vector3(xMove, 0f, zMove).normalized);
            RaycastHit hit;
            for (int i = 0; i < maxReflections; i++)
            {
                rayReflections[i] = ray;
                Physics.Raycast(ray, out hit, Mathf.Infinity, ~playerMask);
                // debugging
                if (showReflectionRays)
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, new Color(1, 1, 1, 0.5f - 0.5f * (float)i / maxReflections));
                }
                if (hit.collider.gameObject.tag == "Speaker")
                {
                    for (int j = i; j > 0; j--)
                    {
                        Physics.Raycast(rayReflections[j].origin + (gameObject.transform.position - rayReflections[j].origin) * -0.0001f, gameObject.transform.position - rayReflections[j].origin, out hit, Mathf.Infinity);
                        if (hit.collider.gameObject.tag == "Player")
                        {
                            // debugging
                            if (showSoundDirectionRays)
                            {
                                Debug.DrawRay(rayReflections[j].origin, gameObject.transform.position - rayReflections[j].origin, new Color(1, 0.75f, 0.75f, 1));
                            }
                            break;
                        }
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
