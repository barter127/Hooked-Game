using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    int m_currentHealth;
    [SerializeField] int m_maxHealth;

    // Reference to script controlling UI.
    [SerializeField] UpdatePlayerHealthUI m_healthScript;

    [SerializeField] GameObject m_GameOverUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentHealth = m_maxHealth;

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Damage")
        {
            TakeDamage(1);
        }
        
        else if (collision.tag == "Max Health")
        {
            AddMaxHealth(1);
        }

        else if (collision.tag == "Heal")
        {
            Heal(1);
        }

        // Update UI.
        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    // Deal damage to player & Update UI.
    void TakeDamage(int damage)
    {
        // Damage will generally always be 1 but -= var offers flexibility.
        m_currentHealth -= damage;

        if (m_currentHealth <= 0)
        {
            // Freeze Game
            Time.timeScale = 0;

            // Death behaviour.
            m_GameOverUI.SetActive(true);
            Destroy(gameObject);
        }
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Heal Item") && m_currentHealth != m_maxHealth)
        {
            HealingAmount healVal = collision.gameObject.GetComponent<HealingAmount>();

            Heal(healVal.HealValue);
            print(healVal.HealValue);

            // Destoy Heal Item.
            Destroy(collision.gameObject);
        }
    }
}
