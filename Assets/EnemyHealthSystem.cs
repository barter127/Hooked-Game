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

    // Getting velocity after collision leads to improper values.
    // Late RB gets the nuber slightly after collision for more proper values
    float m_lateRBVelocity;

    Rigidbody2D m_rigidbody;
    // Inconsistent component referencing but GetComponent acts weirdly and can get the wrong Image. 
    [SerializeField] Image m_healthBar;

    void Start()
    {
        m_currentHealth = m_maxHealth;

        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        m_lateRBVelocity = m_rigidbody.linearVelocity.magnitude;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy has collided with knife obj.
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Tag check might be unessecary and cause me headaches later.
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ApplyDamage(m_lateRBVelocity * m_velocityDamageMultiplier);
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
