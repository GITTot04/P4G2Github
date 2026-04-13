using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject head;
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
        if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, interactionDistance, layerMask))
        {
            Debug.Log("Hit something" + hit.transform.gameObject.name);
            if (hit.transform.tag == "Door")
            {
                Debug.Log("hit door");
                hit.transform.parent.gameObject.GetComponent<Door>().Interact(head.transform.forward);
            }
        }

    }
}
