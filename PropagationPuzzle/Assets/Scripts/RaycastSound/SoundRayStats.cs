using UnityEngine;

[CreateAssetMenu(fileName = "SoundRayStats", menuName = "Scriptable Objects/SoundRayStats")]
public class SoundRayStats : ScriptableObject
{
    public static SoundRayStats instance;
  
    [SerializeField] int maxReflections = 10;
    public int MaxReflections => maxReflections;

    [SerializeField] float maxOcclusions = 5;
    public float MaxOcclusions => maxOcclusions;

    [SerializeField] float occlusionCap = 1;
    public float OcclusionCap => occlusionCap;


    [SerializeField] float intensityChangeSpeed;
    public float IntensityChangeSpeed => intensityChangeSpeed;

    [SerializeField] int bestRayCount;
    public int BestRayCount => bestRayCount;
}
