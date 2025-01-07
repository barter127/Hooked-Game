using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpdateHUD : MonoBehaviour
{
    // Holds public methods to update UI

    public List<Image> m_UIHeartImages = new List<Image>();
    int m_currentUIHealth = 16;

    private void Start()
    {
        // Get reference to every UI heart in scene.
        foreach (GameObject heartObj in GameObject.FindGameObjectsWithTag("UI Heart"))
        {
            if (heartObj.GetComponent<Image>() != null)
            {
                m_UIHeartImages.Add(heartObj.GetComponent<Image>());
            }
            else
            {
                Debug.Log("UI Heart Image is null.");
            }
        }
    }

    // Set Health UI based on parameter.
    public void SetHealthUI(int newHealth) // Floats don't match my game desing ethos. Just use ints.
    {
        //m_healthText.text = newHealth.ToString();
    }

    // Increase or decrease Health UI based on parameter.
    public void IncreaseHealthUI()
    {

    }
}
