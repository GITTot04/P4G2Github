using UnityEngine;

public struct SoundRay // Values
{
    public Vector3 direction;
    public int reflections;
    public int occlusions;
    public SoundRay(Vector3 d, int r, int o, RaycastCheck rayCastCheck)
    {
        direction = d;
        reflections = r;
        occlusions = o;

        AttachDelete(rayCastCheck);
    }
    public void SetValues (Vector3 d, int r, int o, RaycastCheck rayCastCheck)
    {
        direction = d;
        reflections = r;
        occlusions = o;

        AttachDelete(rayCastCheck);
    }
    
    public void AttachDelete(RaycastCheck rayCastCheck)
    {
        rayCastCheck.DeleteRays += Return;
    }

    void Return ()
    {
        SoundRayPool.instance.ReturnSoundRay(this);
    }
    
}
