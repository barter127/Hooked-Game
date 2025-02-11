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

    [SerializeField] GameObject m_coin;

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
        if (collision.CompareTag("Knife") && m_canTakeDamage && !PlayerShootLogic.m_isReturning)
        {
            m_attached = true;
            ApplyDamage(StatisticsScript.m_damage);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Watch to see if knife has returned. Set vars appropriately. Prevents too much coupling.
        if (collision.CompareTag("Knife") && !KnifeEnemyAttachLogic.m_isConnected)
        {
            m_attached = false;
            m_stateMachine.ChangeState(m_stateMachine.m_lastState);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_attached && m_canTakeDamage)
        {
            // Tag check might be unessecary and cause me headaches later.
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Enemy"))
            {
                // Deal damage based on velocity just before.
                // If current velocity is used the damage will be negligble as the Rb has stopped.
                ApplyDamage(m_lateRBVelocity * m_velocityDamageMultiplier);
            }
        }
    }

    // Minus damage from health and update enemy health bar.
    void ApplyDamage(float damage)
    {
        m_currentHealth -= damage;

        UpdateEnemyHealthBar();

        VFXManager.SpawnBloodFX(transform.position);

        // Prevent damage occuring too quickly because of ricochet.
        StartCoroutine(PauseDamageDetection());


        // Allow only one enemy to shake camera.
        if (m_attached)
        {
            VFXManager.ShakeCamera(0.2f);
        }

        // Destory on 0 health.
        if (m_currentHealth <= 0)
        {
            // Update Game Over Stats
            GameOverStatManager.IncrementKillCount();

            EmitCoins();

            // Dead. Bleh X.X
            Destroy(gameObject);
        }

        // At below half health. Switch to weak variant.
        else if (m_currentHealth < m_maxHealth / 2)
        {
            m_stateMachine.ChangeState(StateMachine.AIState.AttachedWeak);
        }

        // Apply DMG VFX. 
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

    // Because of ricocheting rbs AI could insta die. Temporarily sets m_canTakeDamage bool to false.
    IEnumerator PauseDamageDetection()
    {
        m_canTakeDamage = false;

        yield return new WaitForSeconds(m_damageCooldownTime);

        m_canTakeDamage = true;
    }

    // Spawn a random amount of coins based on parameters (inclusive).
    void EmitCoins(int min, int max)
    {
        float coinsToSpawn = Random.Range(min, max);

        for (int i = 0; i < coinsToSpawn; i++)
        {
            Instantiate(m_coin, transform.position, Quaternion.identity);
        }
    }
}
