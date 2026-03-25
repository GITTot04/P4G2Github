using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastCheck : MonoBehaviour
{
    int maxReflections = 10;
    private void Update()
    {
        OnCast();
    }
    public void OnCast()
    {
        Ray ray = new Ray();
        for (float angle = 0; angle < 360; angle += 1)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            float xMove = Mathf.Cos(angleRad);
            float zMove = Mathf.Sin(angleRad);
            ray = new Ray(gameObject.transform.position, new Vector3(xMove, 0f, zMove).normalized);
            RaycastHit hit;
            for (int i = 0; i < maxReflections; i++)
            {
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                // For debugging
                float drawLength = hit.distance;
                Debug.DrawRay(ray.origin, ray.direction * drawLength, new Color(1, 1, 1, 1 - (float)i / maxReflections));
                // ^^
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
        }
    }
}
