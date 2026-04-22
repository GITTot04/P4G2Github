using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public abstract class SoundRayPool : MonoBehaviour
{

}
public abstract class SoundRayPool<T> : SoundRayPool where T : SoundRayPool<T>
{
    public List<SoundRay> pool = new List<SoundRay>();

    public List<SoundRay> ActiveSoundRays = new List<SoundRay>();
    public int raysInPool;
    public int raysInActivePool;

    public static SoundRayPool<T> instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void FixedUpdate()
    {
        raysInPool = pool.Count;
        raysInActivePool = ActiveSoundRays.Count;
    }
    public SoundRay GetSoundRay (Vector3 d, int r, int o)
    {
        if (pool.Count == 0)
        {
            SoundRay soundRay = new SoundRay(d, r, o);
            ActiveSoundRays.Add(soundRay);
            return soundRay;
        }
        else
        {
            SoundRay soundRay = pool[0];
            pool.Remove(soundRay);
            soundRay.SetValues(d, r, o);
            ActiveSoundRays.Add(soundRay);
            return soundRay;
        }


    }
    public void ReturnSoundRay (SoundRay soundRay)
    {
        ActiveSoundRays.Remove(soundRay);
        pool.Add(soundRay);
    }

    public void ReturnAllSoundRays ()
    {
        for (int i = ActiveSoundRays.Count; i > 0; i --)
        {
            ReturnSoundRay(ActiveSoundRays[i - 1]);
        }
    }
}
