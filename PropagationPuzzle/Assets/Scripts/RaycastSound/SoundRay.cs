using UnityEngine;

public struct SoundRay // Values
{
    public Vector3 direction;
    public int reflections;
    public int occlusions;

    public SoundRay(Vector3 d, int r, int o)
    {
        direction = d;
        reflections = r;
        occlusions = o;

    }
    public void SetValues (Vector3 d, int r, int o)
    {
        direction = d;
        reflections = r;
        occlusions = o;

    }
    
   

 
    
}
