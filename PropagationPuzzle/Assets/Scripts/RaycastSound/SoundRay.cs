using UnityEngine;

public struct SoundRay // Values
{
    public Vector3 direction;
    public int reflections;
    public float occlusions;

    public SoundRay(Vector3 d, int r, float o)
    {
        direction = d;
        reflections = r;
        occlusions = o;

    }
    public void SetValues (Vector3 d, int r, float o)
    {
        direction = d;
        reflections = r;
        occlusions = o;

    }
    
   

 
    
}
