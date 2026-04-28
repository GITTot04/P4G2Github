using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;

    Vector2 mouseMovement;
    public Vector2 MouseMovement => mouseMovement;
    Vector2 playerMovement;
    public Vector2 PlayerMovement => playerMovement;

    public delegate void PlayerAction();
    public PlayerAction onAction;
    public PlayerAction onInteraction;
    public PlayerAction onNext;
    public PlayerAction onPrevious;

    public delegate void PlayerActionPlace(string keyName);
    public PlayerActionPlace onPlace;

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
        if (ctx.started)
        {
            if (onAction != null)
            {
                onAction.Invoke();
            }
        }
    }
    public void OnInteraction(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (onInteraction != null)
            {
                onInteraction.Invoke();
            }
        }
    }

    public void OnNumberKey(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (onPlace != null)
            {
                onPlace.Invoke(ctx.control.name);
            }
        }
    }

    public void OnNext (InputAction.CallbackContext ctx)
    {
        if (ctx.started) 
        { 
            if(onNext != null)
            {
                onNext.Invoke();
            }
        }
    }

    public void OnPrevious(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (onPrevious != null)
            {
                onPrevious.Invoke();
            }
        }

    }
}
