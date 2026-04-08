using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    public GameObject playerHead;
    public Camera cam;
    public PlayerStats playerStats;
    Vector2 lastMousePosition;

    void LateUpdate()
    {
        Vector2 rotate = PlayerInput.instance.MouseMovement;

        RotateHorizontal(rotate.x*playerStats.LookSpeed);
        RotateVertical(-rotate.y*playerStats.LookSpeed);
    }

    void RotateHorizontal (float amount)
    {
        playerHead.transform.Rotate(new Vector3(0f, amount, 0f));
    }

    void RotateVertical (float amount)
    {
        if (transform.rotation.eulerAngles.x + amount < 90 || transform.rotation.eulerAngles.x + amount > 270)
        {
            cam.transform.Rotate(new Vector3(amount, 0f, 0f));
        }
        
    }
}
