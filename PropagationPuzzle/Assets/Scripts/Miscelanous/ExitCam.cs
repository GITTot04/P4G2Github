using System.Collections;
using System.Threading;
using UnityEngine;

public class ExitCam : MonoBehaviour
{
    public GameObject exitCam;
    public float timer;

    public void ExitDoor()
    {
        
        exitCam.SetActive(true);
        StartCoroutine(Timer());
    }
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);

       
        exitCam.SetActive(false);
    }
}
