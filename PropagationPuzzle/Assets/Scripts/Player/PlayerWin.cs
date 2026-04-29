using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerWin : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ExitDoor" && DoorManager.instance.hasWon)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            DoorManager.instance.openDoorCount = 0;
        }
    }
}
