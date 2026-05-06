using UnityEngine;
using TMPro;

public class WinText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Update()
    {
        if (text.fontSize < 280)
        {
            text.fontSize += 12f * Time.deltaTime;
        }
    }
}
