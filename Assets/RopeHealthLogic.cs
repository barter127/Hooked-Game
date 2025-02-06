using UnityEngine;
using UnityEngine.UI;

public class RopeHealthLogic : MonoBehaviour
{
    /// <summary>
    /// Handles animations and contains methods for updating rope health bar.
    /// Because of project scope there won't be any bosses but still design script with that intent.
    /// </summary>

    // Components
    [SerializeField] Image m_healthBar;

    // Animation Vars.
    bool m_inStartAnimation = false;
    float m_barFillAmount = 0;
    float m_animationSpeed = 4f;

    void OnEnable()
    {
        // Set parameters for start up animation
        m_inStartAnimation = true;
        m_barFillAmount = 0;
        m_healthBar.fillAmount = m_barFillAmount;
    }

    void Update()
    {
        // Start up fill animation.
        if (m_inStartAnimation )
        {
            // Fill health bar using delta.
            m_barFillAmount += Time.deltaTime * m_animationSpeed;
            m_healthBar.fillAmount = m_barFillAmount;

            // Check health bar is full.
           if (m_barFillAmount > 1)
            {
                m_inStartAnimation = false;
            }
        }
    }

    public void UpdateHealthBar()
    {

    }
}
