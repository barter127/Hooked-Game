using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpdateHUD : MonoBehaviour
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

    // Start to ensure hearts are loaded.
    private void Start()
    {
        // Initialise Health UI.
        m_currentUIHealth = m_maxUIHealth;

        SetHealthUI(m_maxUIHealth);
        UpdateNullHearts();
    }

    public void UpdateHealthUI()
    {
        for (int i = 0; i < m_maxUIHealth / 2; i++)
        {
            // Convert i to matching health value.
            int healthIndex = (i + 1) * 2;

            // Heart is full
            if (m_currentUIHealth >= healthIndex)
            {
                m_uiHeartImages[i].sprite = m_fullHeartImg;
            }
            // Has half a heart
            else if (m_currentUIHealth == healthIndex - 1)
            {
                m_uiHeartImages[i].sprite = m_halfHeartImg;
            }
            // Heart is empty but not null.
            else
            {
                m_uiHeartImages[i].sprite = m_emptyHeartImg;
            }
        }

        UpdateNullHearts();
    }

    // Set hearts to null sprite in UI.
    public void UpdateNullHearts()
    {
        for (int i = m_maxUIHealth / 2; i < m_uiHeartImages.Count; i++)
        {
            m_uiHeartImages[i].sprite = m_nullHeartImg;
        }
    }

    // Set Health UI based on parameter.
    public void SetHealthUI(int newHealth) // Floats don't match my game design ethos. Just use ints.
    {
        m_currentUIHealth = newHealth;
        m_currentUIHealth = Mathf.Clamp(m_currentUIHealth, 0, m_maxUIHealth);

        UpdateHealthUI();
    }


    // Increase or decrease Health UI based on parameter.
    public void IncreaseHealthUI(int increase)
    {
        SetHealthUI(m_currentUIHealth + increase);
    }

    // Set max health based on parametrer.
    public void SetMaxHealthUI(int newMax, int newCurrentHealth)
    {
        // Ensure newMax is a multiple of 2.
        if (newMax % 2 != 0)
        {
            newMax++;
        }

        // Update newMax.
        newMax = Mathf.Clamp(newMax, 0, m_uiHeartImages.Count);
        m_maxUIHealth = newMax;

        // Update currentUIHealth.
        m_currentUIHealth = newCurrentHealth;

        UpdateHealthUI();
    }

    // Display the new max UI health
    public void IncreaseMaxHealthUI(int maxIncrease, int currentIncrease)
    {
        // Ensure increase is multiple of 2.
        if (maxIncrease % 2 != 0)
        {
            maxIncrease++;
        }

        SetMaxHealthUI(m_maxUIHealth + maxIncrease, m_currentUIHealth + currentIncrease);
    }
}
