using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSystem : MonoBehaviour
{
    // Apply damage on appropriate collision.
    // Update Healthbar UI.
    // Destroy at health <= 0.

    float m_currentHealth;
    [SerializeField] float m_maxHealth;

    [SerializeField] Image m_healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        m_currentHealth -= 0.1f;
        UpdateEnemyHealthBar();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void UpdateEnemyHealthBar()
    {
        float healthPercentage = m_currentHealth / m_maxHealth;
        m_healthBar.fillAmount = healthPercentage;
    }
}
