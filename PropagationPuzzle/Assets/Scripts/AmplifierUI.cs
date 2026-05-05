using UnityEngine;
using UnityEngine.UI;

public class AmplifierUI : MonoBehaviour
{
    public GameObject amp1ImageObject;
    public GameObject amp2ImageObject;
    public GameObject amp3ImageObject;
    Image amp1Image;
    Image amp2Image;
    Image amp3Image;
    private void Awake()
    {
        amp1Image = amp1ImageObject.GetComponent<Image>();
        amp2Image = amp2ImageObject.GetComponent<Image>();
        amp3Image = amp3ImageObject.GetComponent<Image>();
    }

    public void UpdateAmplifierUI(string amplifierNumber, bool setActiveStatus)
    {
        switch (amplifierNumber)
        {
            case "1":
                if (setActiveStatus)
                {
                    amp1Image.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    amp1Image.color = new Color(1f, 1f, 1f, 0.5f);
                }
                break;
            case "2":
                if (setActiveStatus)
                {
                    amp2Image.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    amp2Image.color = new Color(1f, 1f, 1f, 0.5f);
                }
                break;
            case "3":
                if (setActiveStatus)
                {
                    amp3Image.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    amp3Image.color = new Color(1f, 1f, 1f, 0.5f);
                }
                break;
            default:
                break;
        }
    }
    public void SetAmplifierUI(int amountOfAmplifiers)
    {
        switch (amountOfAmplifiers)
        {
            case 0:
                amp1ImageObject.SetActive(false);
                amp2ImageObject.SetActive(false);
                amp3ImageObject.SetActive(false);
                break;
            case 1:
                amp1ImageObject.SetActive(true);
                amp2ImageObject.SetActive(false);
                amp3ImageObject.SetActive(false);
                break;
            case 2:
                amp1ImageObject.SetActive(true);
                amp2ImageObject.SetActive(true);
                amp3ImageObject.SetActive(false);
                break;
            case 3:
                amp1ImageObject.SetActive(true);
                amp2ImageObject.SetActive(true);
                amp3ImageObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
