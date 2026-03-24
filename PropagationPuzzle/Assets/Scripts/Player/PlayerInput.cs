using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;

    Vector2 mouseMovement;
    public Vector2 MouseMovement => mouseMovement;
    Vector2 playerMovement;
    public Vector2 PlayerMovement => playerMovement;
    void Awake ()
    {
        if (instance != this)
        {
            instance = this;
        }
    }
    void Update()
    {
        // mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        playerMovement = ctx.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        mouseMovement = ctx.ReadValue<Vector2>();
    }

    public void OnAction(InputAction.CallbackContext ctx)
    {

    }
}
