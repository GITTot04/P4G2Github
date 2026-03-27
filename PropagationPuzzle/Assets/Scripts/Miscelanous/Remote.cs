using UnityEngine;

public class Remote : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        PlayerInput.instance.onAction += PushButton;
    }

    void PushButton() 
    {
        animator.SetTrigger("Push");
    }
}
