using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(StateMachine))]
public class EnemyHealthSystem : MonoBehaviour
{
    /// <summary>
    /// Apply damage on appropriate collision.
    /// Update Healthbar UI.
    /// Destroy at health <= 0.
    /// </summary>

    StateMachine m_stateMachine;

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

    // State Machine Reference.

    void Start()
    {
        m_currentHealth = m_maxHealth;

        m_stateMachine = GetComponent<StateMachine>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    { 
        m_lateRBVelocity = m_rigidbody.linearVelocity.magnitude;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy has collided with knife obj.
        if (collision.CompareTag("Knife") && PlayerShootLogic.m_hasFired && m_canTakeDamage)
        {
            // Get script holding DMG nums.
            StatisticsScript stats = collision.gameObject.GetComponent<StatisticsScript>();

            m_stateMachine.ChangeState(StateMachine.AIState.Attached);

            if (stats != null)
            {
                m_attached = true;
                ApplyDamage(stats.damage, m_attached);
            }
            else
            {
                Debug.Log("Player stats invalid couldn't apply damage");
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Knife") && !KnifeEnemyAttachLogic.m_isConnected)
        {
            m_attached = false;
            m_stateMachine.ChangeState(m_stateMachine.m_lastState);

            StartCoroutine(PauseDamageDetection());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_attached && m_canTakeDamage)
        {
            // Tag check might be unessecary and cause me headaches later.
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
            {
                // Deal damage based on velocity before.
                ApplyDamage(m_lateRBVelocity * m_velocityDamageMultiplier, m_attached);

                Debug.Log(m_lateRBVelocity * m_velocityDamageMultiplier);
                Debug.Log(m_lateRBVelocity);

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
        

        // Allow only one enemy to shake camera.
        if (attached)
        {
            VFXManager.ShakeCamera(0.2f);
        }

        
        if (m_currentHealth < m_maxHealth / 2)
        {
            m_stateMachine.ChangeState(StateMachine.AIState.AttachedWeak);
        }

        // Destory on 0 health.
        if (m_currentHealth <= 0)
        {
            // Update Game Over Stats
            GameOverStatManager.IncrementKillCount();

            // Maybe switch to object pooling.
            Destroy(gameObject);
        }

        // Tries to set destroyed component otherwise. 
        else
        {
            VFXManager.FlashRed(m_spriteRenderer, 0.2f);
        }
    }

    // Update health bar.
    void UpdateEnemyHealthBar()
    {
        // Calculate health decimal. Set bar fill to that value.
        float healthPercentage = m_currentHealth / m_maxHealth;
        m_healthBar.fillAmount = healthPercentage;
    }

    // Because of ricocheting rbs AI could instadie. Temporarily sets bool to false.
    IEnumerator PauseDamageDetection()
    {
        m_canTakeDamage = false;

        yield return new WaitForSeconds(m_damageCooldownTime);

        m_canTakeDamage = true;
    }
}
