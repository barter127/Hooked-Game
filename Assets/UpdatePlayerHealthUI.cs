using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpdatePlayerHealthUI : MonoBehaviour
{
    // Holds public methods to update UI
    // REMEMBER: health variables refer to amount of hits so 8 = the 4th heart.

    [Header ("Health Component Reference")]
    [SerializeField] List<Image> m_uiHeartImages = new List<Image>();
    public int m_currentUIHealth;
    int m_maxUIHealth = 8;

    [Header("Health Images")]
    [SerializeField] Sprite m_fullHeartImg;
    [SerializeField] Sprite m_halfHeartImg;
    [SerializeField] Sprite m_emptyHeartImg;

    [SerializeField] Sprite m_nullHeartImg; // Indicate health that can be gained.

    public void UpdateHealthUI(int current, int max)
    {
        for (int i = 0; i < max / 2; i++)
        {
            // Convert i to matching health value.
            int healthIndex = (i + 1) * 2;

            // Heart is full
            if (current >= healthIndex)
            {
                m_uiHeartImages[i].sprite = m_fullHeartImg;
            }
            // Has half a heart
            else if (current == healthIndex - 1)
            {
                m_uiHeartImages[i].sprite = m_halfHeartImg;
            }
            // Heart is empty but not null.
            else
            {
                m_uiHeartImages[i].sprite = m_emptyHeartImg;
            }
        }

        UpdateNullHearts(max);
    }

    // Set hearts to null sprite in UI.
    public void UpdateNullHearts(int maxHealth)
    {
        for (int i = maxHealth / 2; i < m_uiHeartImages.Count; i++)
        {
            m_uiHeartImages[i].sprite = m_nullHeartImg;
        }
    }
}
