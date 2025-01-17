using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSystem : MonoBehaviour
{
    /// <summary>
    /// Apply damage on appropriate collision.
    /// Update Healthbar UI.
    /// Destroy at health <= 0.
    /// </summary>

    float m_currentHealth;
    [SerializeField] float m_maxHealth;

    [SerializeField] Image m_healthBar;

    void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Knife"))
        {
            StatisticsScript stats = collision.gameObject.GetComponent<StatisticsScript>();

            m_currentHealth -= stats.damage;
        }

        else if (collision.gameObject.CompareTag("Obstacle"))
        {

        }

        m_currentHealth -= 0.1f;
        UpdateEnemyHealthBar();
    }

    void UpdateEnemyHealthBar()
    {
        float healthPercentage = m_currentHealth / m_maxHealth;
        m_healthBar.fillAmount = healthPercentage;
    }
}
