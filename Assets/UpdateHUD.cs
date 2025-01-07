using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpdateHUD : MonoBehaviour
{
    // Holds public methods to update UI

    [Header ("Health Component Reference")]
    [SerializeField] List<Image> m_uiHeartImages = new List<Image>();
    int m_currentUIHealth = 8;
    int m_maxUIHealth = 8;

    [Header("Health Images")]
    [SerializeField] Sprite m_fullHeartImg;
    [SerializeField] Sprite m_halfHeartImg;
    [SerializeField] Sprite m_emptyHeartImg;

    [SerializeField] Sprite m_nullHeartImg; // Indicate health that can be gained.

    // Set Health UI based on parameter.
    public void SetHealthUI(int newHealth) // Floats don't match my game design ethos. Just use ints.
    {
        for (int i = 0; i < m_maxUIHealth; i++)
        {
            int healthIndex = (i + 1) * 2;

            if (newHealth >= healthIndex)
            {
                m_uiHeartImages[i].sprite = m_fullHeartImg;
            }
            else if (newHealth == healthIndex - 1)
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
    }
}
