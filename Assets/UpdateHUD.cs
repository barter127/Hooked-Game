using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpdateHUD : MonoBehaviour
{
    // Holds public methods to update UI

    [Header ("Health Component Reference")]
    public List<Image> m_UIHeartImages = new List<Image>();
    int m_currentUIHealth = 8;
    int m_currentMaxUIHealth = 8;

    [Header("Health Images")]
    [SerializeField] Sprite m_fullHeartImg;
    [SerializeField] Sprite m_halfHeartImg;
    [SerializeField] Sprite m_emptyHeartImg;

    [SerializeField] Sprite m_nullHeartImg; // Indicate health that can be gained.

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
    public void SetHealthUI(int newHealth) // Floats don't match my game design ethos. Just use ints.
    {
        for (int i = 0; i < m_UIHeartImages.Count; i++)
        {

        }

        //m_healthText.text = newHealth.ToString();
    }

    // Increase or decrease Health UI based on parameter.
    public void IncreaseHealthUI(int healthChange)
    {
    }
}
