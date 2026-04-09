using UnityEngine;

[CreateAssetMenu(fileName = "SoundRayStats", menuName = "Scriptable Objects/SoundRayStats")]
public class SoundRayStats : ScriptableObject
{
    public static SoundRayStats instance;
  
    [SerializeField] int maxReflections = 10;
    public int MaxReflections => maxReflections;

    [SerializeField] int maxOcclusions = 5;
    public int MaxOcclusions => maxOcclusions;

    [SerializeField] float occlusionCap = 1;
    public float OcclusionCap => occlusionCap;

    [SerializeField] float maxIntensity;
    public float MaxIntensity => maxIntensity;

    [SerializeField] float intensityVolume;
    public float IntensityVolume => intensityVolume;

}
