using FMODUnity;
using UnityEngine;

public class OcclusionManipulation : MonoBehaviour
{
    public float occlusionValue; // Initial occlusion value
    public float minOcclusionValue;
    public float maxOcclusionValue;
    public GameObject emitter;

    public void Start()
    {
        if (emitter == null)
        {
            Debug.Log("Emitter GameObject is not assigned.");
            emitter = FindFirstObjectByType<StudioEventEmitter>()?.gameObject;
        }
        
    }
    public void Update()
    {
        if (emitter != null)
        {
            // Set the occlusion value for the emitter
            StudioEventEmitter eventEmitter = emitter.GetComponent<StudioEventEmitter>();
            if (eventEmitter != null)
            {
                occlusionValue = Mathf.Clamp(occlusionValue, minOcclusionValue, maxOcclusionValue); // Ensure occlusion value is between 0 and 1
                eventEmitter.GetComponent<StudioEventEmitter>().occlusionIntensity = occlusionValue;
                //eventEmitter.SetParameter("Occlusion", occlusionValue);
            }
        }
    }
}
