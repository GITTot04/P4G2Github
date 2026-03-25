using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public LayerMask layerMask;
    public float interactionDistance = 1f;
    void Start()
    {
        PlayerInput.instance.onInteraction += CheckInteraction;
    }

    void Update()
    {
        
    }
    void CheckInteraction ()
    {
        Debug.Log("interacting");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, layerMask))
        {
            if (hit.transform.tag == "Door")
            {
                Debug.Log("hit door");
                hit.transform.parent.gameObject.GetComponent<Door>().Interact(transform.forward);
            }
        }

    }
}
