using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SoundRayPool : MonoBehaviour
{
    public List<SoundRay> pool =  new List<SoundRay>();
    public static SoundRayPool instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public SoundRay GetSoundRay (Vector3 d, int r, int o, RaycastCheck rayCastCheck)
    {
        if (pool.Count == 0)
        {
            SoundRay soundRay = new SoundRay(d, r, o, rayCastCheck);
            return soundRay;
        }
        else
        {
            SoundRay soundRay = pool[0];
            soundRay.SetValues(d, r, o, rayCastCheck);
            return soundRay;
        }


    }
    public void ReturnSoundRay (SoundRay soundRay)
    {
        pool.Add(soundRay);
    }
}
