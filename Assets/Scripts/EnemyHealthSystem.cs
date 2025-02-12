using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent (typeof(StateMachine))]
public class EnemyHealthSystem : MonoBehaviour
{
    /// <summary>
    /// Apply damage on appropriate collision.
    /// Update Healthbar UI.
    /// Destroy at health <= 0.
    /// </summary>

    [Header ("Enemy Drops")]
    [SerializeField] GameObject m_coin;
    [SerializeField] GameObject m_healingItem;

    [Header ("Components")]
    [SerializeField] Rigidbody2D m_rigidbody;
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] Image m_healthBar;

    // Hold AI current State + Chnage State method.
    [SerializeField] StateMachine m_stateMachine;

    [SerializeField] EnemySpawner m_enemySpawner;

    [Header ("Gameplay Vars")]
    float m_currentHealth;
    [SerializeField] float m_maxHealth;

    bool m_attached = false;
    [SerializeField] bool m_canTakeDamage = true;

    // Multiplies damage from rb velocity.
    float m_velocityDamageMultiplier = 10;

    // Getting velocity after collision leads to improper values.
    // Late RB gets the nuber slightly after collision for more proper values
    float m_lateRBVelocity;

    float m_damageCooldownTime = 0.5f;

    // -- Spawner Communication --

    public enum EnemyType
    {
        Hornet,
        Wasp,
        Squirt,
        Dip,
        None // Just incase.
    }

    // Enemy type of the owner of this script.
    public EnemyType m_ownerEnemyType;

    public UnityEvent m_onDeathEvent;

    // State Machine Reference.

    void Start()
    {
        m_currentHealth = m_maxHealth;

        // Everyday we stray further from God.
        m_enemySpawner = GameObject.Find("Enemy Spawn Manager").GetComponent<EnemySpawner>();

        // Set subscriber based on event. So enemy spawner knows when the specific enemy dies
        SetDeathEventSubscriber();

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

        // Apply DMG FX.
        VFXManager.FlashRed(m_spriteRenderer, 0.2f);
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

            DropItem();
            EmitCoins(1, 10);

            m_onDeathEvent.Invoke();

            // Dead. Bleh X.X
            Destroy(gameObject);
        }

        // At below half health. Switch to weak variant.
        else if (m_currentHealth < m_maxHealth / 2)
        {
            m_stateMachine.ChangeState(StateMachine.AIState.AttachedWeak);
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

    // Rolls random number drops corresponding item on death (or nothing).
    void DropItem()
    {
        float randomNum = Random.Range(0, 20);


        // Convert to switch if more items can be dropped.
        if (randomNum == 0)
        {
            Instantiate(m_healingItem, transform.position, Quaternion.identity);
        }
    }

    // Subscribe a method to the spawner event so the spawner variables can be decremented.
    void SetDeathEventSubscriber()
    {
        switch (m_ownerEnemyType)
        {
            case EnemyType.Hornet: m_onDeathEvent.AddListener(m_enemySpawner.DecrementHornetCount); break;
            case EnemyType.Wasp: m_onDeathEvent.AddListener(m_enemySpawner.DecrementWaspCount); break;
            case EnemyType.Squirt: m_onDeathEvent.AddListener(m_enemySpawner.DecrementSquirtCount); break;
            case EnemyType.Dip: m_onDeathEvent.AddListener(m_enemySpawner.DecrementDipCount); break;

            default: Debug.Log("Enemy type not correctly assigned" + gameObject.name); break;
        }

        Debug.Log("Freddy Fazburger");
    }
}
