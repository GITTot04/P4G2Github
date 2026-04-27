using UnityEngine;

public class ExitDoor : MonoBehaviour

{
    public GameObject Indicator;
    public GameObject DoorLight;

    bool open = false;
    public bool IsOpen => open;

    public float openDirection;

    float startRotation;
    float targetAngle;
    float escapeAngle;
    public float timeOfOperation;

    bool inProgress = false;
    float progressTimer = 0;

    public GameObject Hinge;


    void Start()
    {
        startRotation = Hinge.transform.rotation.eulerAngles.y;
    }


    public void UnlockDoor()
    {
        if (openDirection < 0)
        {
            targetAngle = startRotation + 90;
        } else
        {
            targetAngle = startRotation - 90;
        }
        open = true;
        inProgress = true;
        progressTimer = 0f;
        escapeAngle = startRotation;

        Indicator.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 255);
        DoorLight.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 255, 0, 255));
    }



    void FixedUpdate()
    {
        if (inProgress)
        {
            float newRotation = Mathf.LerpAngle(escapeAngle, targetAngle, progressTimer);
            Hinge.transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, newRotation, transform.rotation.eulerAngles.z));
         
            progressTimer += Time.fixedDeltaTime / timeOfOperation; 
         
        }
    }
}
