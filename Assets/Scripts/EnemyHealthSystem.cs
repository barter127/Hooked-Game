using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSystem : MonoBehaviour
{
    /// <summary>
    /// Apply damage on appropriate collision.
    /// Update Healthbar UI.
    /// Destroy at health <= 0.
    /// </summary>

    [SerializeField] CameraMovement m_camMovement;

    // --- Components ---
    Rigidbody2D m_rigidbody;
    SpriteRenderer m_spriteRenderer;

    // Inconsistent component referencing but GetComponent acts weirdly and can get the wrong Image. 
    [SerializeField] Image m_healthBar;

    // --- Gameplay Vars ---
    float m_currentHealth;
    [SerializeField] float m_maxHealth;

    bool m_attached = false;
    [SerializeField] bool m_canTakeDamage = true;

    [SerializeField] GameObject m_bloodFX;

    // Multiplies damage from rb velocity.
    float m_velocityDamageMultiplier = 10;

    // Getting velocity after collision leads to improper values.
    // Late RB gets the nuber slightly after collision for more proper values
    float m_lateRBVelocity;

    float m_damageCooldownTime = 0.5f;



    void Start()
    {
        m_currentHealth = m_maxHealth;

        m_rigidbody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    { 
        if (m_attached)
        {
            m_lateRBVelocity = m_rigidbody.linearVelocity.magnitude;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy has collided with knife obj.
        if (collision.CompareTag("Knife"))
        {
            StatisticsScript stats = collision.gameObject.GetComponent<StatisticsScript>();

            if (stats != null)
            {
                m_attached = true;
                ApplyDamage(stats.damage, m_attached);
            }
            else
            {
                Debug.Log("Enemy stats invalid couldn't apply damage");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_canTakeDamage || m_attached)
        {
            // Tag check might be unessecary and cause me headaches later.
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
            {
                ApplyDamage(m_lateRBVelocity * m_velocityDamageMultiplier, m_attached);

                StartCoroutine(PauseDamageDetection());
            }
        }
    }

    // Minus damage from health and update health bar.
    void ApplyDamage(float damage, bool attached)
    {
        Instantiate(m_bloodFX, transform.position, Quaternion.identity);

        m_currentHealth -= damage;

        UpdateEnemyHealthBar();

        VFXManager.SpawnBloodFX(transform.position);
        VFXManager.FlashRed(m_spriteRenderer ,0.2f);
        

        // Only shake cam if appropriate.
        if (attached)
        {
            VFXManager.ShakeCamera(0.2f);
        }

        // Destory on 0 health.
        if (m_currentHealth <= 0)
        {
            // Update Game Over Stats
            GameOverStatManager.IncrementKillCount();

            // Maybe switch to object pooling.
            Destroy(gameObject);
        }
    }

    // Update health bar.
    void UpdateEnemyHealthBar()
    {
        // Calculate health decimal. Set bar fill to that value.
        float healthPercentage = m_currentHealth / m_maxHealth;
        m_healthBar.fillAmount = healthPercentage;
    }

    // 
    IEnumerator PauseDamageDetection()
    {
        m_canTakeDamage = false;

        yield return new WaitForSeconds(m_damageCooldownTime);

        m_canTakeDamage = true;
    }
}
