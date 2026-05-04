using System.Collections;
using System.Threading;
using UnityEngine;

public class ExitCam : MonoBehaviour
{
    public GameObject playerCam;
    public GameObject remoteCam;
    public GameObject exitCam;
    public float timer;


    public void ExitDoor()
    {
        playerCam.SetActive(false);
        remoteCam.SetActive(false);
        exitCam.SetActive(true);
        StartCoroutine(Timer());
    }
    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(timer);
        playerCam.SetActive(true);
        remoteCam.SetActive(true);
        exitCam.SetActive(false);
    }
}
