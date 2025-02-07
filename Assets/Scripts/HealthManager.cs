using System.Collections;
using UnityEngine;


public class HealthManager : MonoBehaviour
{
    int m_currentHealth;
    [SerializeField] int m_maxHealth;

    bool m_canTakeDamage = true;
    [SerializeField] float m_iFrameLength;

    // Reference to script controlling UI.
    [SerializeField] UpdatePlayerHealthUI m_healthScript;

    [SerializeField] GameObject m_GameOverUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentHealth = m_maxHealth;

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Heal Item") && m_currentHealth != m_maxHealth)
        {
            HealingAmount healVal = collision.gameObject.GetComponent<HealingAmount>();

            Heal(healVal.HealValue);

            // Destoy Heal Item.
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }

        else if (collision.gameObject.CompareTag("Max Health"))
        {
            AddMaxHealth(1);
        }
    }

    // Deal damage to player & Update UI.
    void TakeDamage(int damage)
    {
        if (m_canTakeDamage)
        {
            // Damage will generally always be 1 but -= var offers flexibility.
            m_currentHealth -= damage;
            m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);

            StartCoroutine(IFrames(m_iFrameLength));

            if (m_currentHealth <= 0)
            {
                // Freeze Game
                Time.timeScale = 0;

                // Death behaviour.
                m_GameOverUI.SetActive(true);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator IFrames(float length)
    {
        m_canTakeDamage = false;

        yield return new WaitForSeconds(length);

        m_canTakeDamage = true;
    }

    void Heal(int increase)
    {
        m_currentHealth += increase;
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    // Increase players max health and current health by param.
    void AddMaxHealth(int maxIncrease)
    {
        m_maxHealth += maxIncrease;

        // Ensure value is multiple of 2.
        if (m_maxHealth % 2 != 0)
        {
            m_maxHealth++;
            m_currentHealth += maxIncrease * 2;
        }
        else
        {
            m_currentHealth += maxIncrease;
        }

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    // Increase players max health by maxIncrease and currentHealth by currentIncrease.
    void AddMaxHealth(int maxIncrease, int currentIncrease)
    {
        m_maxHealth += maxIncrease;

        // Ensure value is multiple of 2.
        if (m_maxHealth % 2 != 0)
        {
            m_maxHealth++;
        }

        m_currentHealth += currentIncrease;

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }
}
