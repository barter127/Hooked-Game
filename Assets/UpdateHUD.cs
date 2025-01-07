using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpdateHUD : MonoBehaviour
{
    // Holds public methods to update UI

    [Header ("Health Component Reference")]
    [SerializeField] List<Image> m_uiHeartImages = new List<Image>();
    public int m_currentUIHealth;
    int m_maxUIHealth = 8;

    [Header("Health Images")]
    [SerializeField] Sprite m_fullHeartImg;
    [SerializeField] Sprite m_halfHeartImg;
    [SerializeField] Sprite m_emptyHeartImg;

    [SerializeField] Sprite m_nullHeartImg; // Indicate health that can be gained.

    private void Awake()
    {
        // Initialise Health UI.
        m_currentUIHealth = m_maxUIHealth;
        SetHealthUI(m_maxUIHealth);
    }

    // Set Health UI based on parameter.
    public void SetHealthUI(int newHealth) // Floats don't match my game design ethos. Just use ints.
    {
        m_currentUIHealth = newHealth;
        m_currentUIHealth = Mathf.Clamp(m_currentUIHealth, 0, m_maxUIHealth);

        for (int i = 0; i < m_maxUIHealth; i++)
        {
            int healthIndex = (i + 1) * 2;

            if (m_currentUIHealth >= healthIndex)
            {
                m_uiHeartImages[i].sprite = m_fullHeartImg;
            }
            else if (m_currentUIHealth == healthIndex - 1)
            {
                m_uiHeartImages[i].sprite = m_halfHeartImg;
            }
            else
            {
                m_uiHeartImages[i].sprite = m_emptyHeartImg;
            }
        }
    }


    // Increase or decrease Health UI based on parameter.
    public void IncreaseHealthUI(int healthChange)
    {
        SetHealthUI(m_currentUIHealth + healthChange);
    }
}
