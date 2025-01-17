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

    // Multiplies damage from rb velocity.
    [SerializeField] float m_velocityDamageMultiplier;

    Rigidbody2D m_rigidbody;
    // Inconsistent component referencing but GetComponent acts weirdly and can get the wrong Image. 
    [SerializeField] Image m_healthBar;

    void Start()
    {
        m_currentHealth = m_maxHealth;

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Enemy has collided with knife obj.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knife"))
        {
            StatisticsScript stats = collision.gameObject.GetComponent<StatisticsScript>();

            if (stats != null)
            {
                ApplyDamage(stats.damage);
                UpdateEnemyHealthBar();
            }
            else
            {
                Debug.Log("Enemy stats invalid couldn't apply damage");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Tag check might be unessecary and cause me headaches later.
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ApplyDamage(m_rigidbody.linearVelocity.magnitude * m_velocityDamageMultiplier);
            Debug.Log(m_rigidbody.linearVelocity.magnitude);
        }
    }

    void ApplyDamage(float damage)
    {
        m_currentHealth -= damage;
        UpdateEnemyHealthBar();

        // Destory on 0 health.
        if (m_currentHealth <= 0)
        {
            // Maybe switch to object pooling.
            Destroy(gameObject);
        }
    }

    void UpdateEnemyHealthBar()
    {
        float healthPercentage = m_currentHealth / m_maxHealth;
        m_healthBar.fillAmount = healthPercentage;
    }
}
