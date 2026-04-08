using UnityEngine;

[CreateAssetMenu(fileName = "SoundRayStats", menuName = "Scriptable Objects/SoundRayStats")]
public class SoundRayStats : ScriptableObject
{
    public static SoundRayStats instance;
  
    [SerializeField] int maxReflections = 10;
    public int MaxReflections => maxReflections;

    [SerializeField] int maxOcclusions = 5;
    public int MaxOcclusions => maxOcclusions;
}
