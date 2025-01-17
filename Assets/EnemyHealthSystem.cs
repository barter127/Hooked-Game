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
        Debug.Log(collision.gameObject.name);  
        Debug.Log(collision.gameObject.CompareTag("Knife"));

        if (collision.gameObject.CompareTag("Knife"))
        {
            StatisticsScript stats = collision.gameObject.GetComponent<StatisticsScript>();

            if (stats != null)
            {
                m_currentHealth -= stats.damage;
                UpdateEnemyHealthBar();
            }
            else
            {
                Debug.Log("Enemy stats invalid couldn't apply damage");
            }
        }
    }

    void UpdateEnemyHealthBar()
    {
        float healthPercentage = m_currentHealth / m_maxHealth;
        m_healthBar.fillAmount = healthPercentage;
    }
}
