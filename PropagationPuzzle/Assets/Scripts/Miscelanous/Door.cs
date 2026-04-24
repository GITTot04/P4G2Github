using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject Indicator;
    public GameObject DoorLight;

    bool open = false;
    public bool IsOpen => open;

    float startRotation;
    float targetAngle;
    float escapeAngle;
    public float timeOfOperation;

    bool inProgress = false;
    float progressTimer = 0;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.y;
    }

    public void Interact(Vector3 viewDiretion)
    {
        if (!inProgress)
        {
            escapeAngle = transform.rotation.eulerAngles.y;

            if (open) Close();
            else Open(viewDiretion);
        }
    }

    void Open(Vector3 viewDirection)
    {

        if (Vector3.Dot(viewDirection, transform.forward) < 0)
        {
            targetAngle = startRotation + 90;
        } else
        {
            targetAngle = startRotation - 90;
        }
        open = true;
        inProgress = true;
        progressTimer = 0f;

        DoorManager.instance.openDoorCount++;
    }

    void Close()
    {

        open = false;
        targetAngle = startRotation;
        inProgress = true;
        progressTimer = 0f;
 
    }


    void FixedUpdate()
    {
        if (inProgress)
        {
            float newRotation = Mathf.LerpAngle(escapeAngle, targetAngle, progressTimer);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, newRotation, transform.rotation.eulerAngles.z));
         
            progressTimer += Time.fixedDeltaTime / timeOfOperation; 
            if (progressTimer >= 1f)
            {
                inProgress = false;
                if (open)
                {


                    Indicator.gameObject.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 255);
                    DoorLight.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(0, 255, 0, 255));
                } 
                else
                {
                    DoorManager.instance.openDoorCount--;

                    Indicator.gameObject.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
                    DoorLight.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 0, 0, 255));
                }
            }
        }
    }
}
