using System.Collections;
using UnityEngine;


public class HealthManager : MonoBehaviour
{
    // Health
    int m_currentHealth;
    [SerializeField] int m_maxHealth;

    // IFrames.
    bool m_canTakeDamage = true;

    // Length of INVINCIBLE ity
    [SerializeField] float m_iFrameLength;

    // Number of times VFX will flash.
    [SerializeField] int m_iFrameFlashes;
    SpriteRenderer m_spriteRenderer;

    // UI.
    [SerializeField] UpdatePlayerHealthUI m_healthScript;
    [SerializeField] GameObject m_GameOverUI;

    [Header("SFX")]
    [SerializeField] AudioSource m_audioSource;

    [SerializeField] AudioClip[] m_hurtSounds;
    [SerializeField] AudioClip m_healSFX;
    [SerializeField] AudioClip m_deathSFX;

    void Start()
    {
        m_currentHealth = m_maxHealth;

        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Collider heals and player isn't at max health.
        if (collision.gameObject.CompareTag("Heal Item") && m_currentHealth != m_maxHealth)
        {
            HealingAmount healVal = collision.gameObject.GetComponent<HealingAmount>();

            Heal(healVal.HealValue);

            // Destoy Heal Item.
            Destroy(collision.gameObject);
        }

        // If collider deals damage
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Damage"))
        {
            TakeDamage(1);
        }

        else if (collision.gameObject.CompareTag("Max Health"))
        {
            AddMaxHealth(1);
        }
    }

    #region Health Manipulation Methods
    // Deal damage to player & Update UI.
    void TakeDamage(int damage)
    {
        if (m_canTakeDamage)
        {
            // Damage will generally always be 1 but -= var offers flexibility.
            m_currentHealth -= damage;
            m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);

            PlayRandomHurtSFX(m_hurtSounds);

            StartCoroutine(IFrames(m_iFrameLength));

            VFXManager.IFrames(m_spriteRenderer, m_iFrameFlashes, m_iFrameLength);

            if (m_currentHealth <= 0)
            {
                // Freeze Game
                Time.timeScale = 0;

                m_audioSource.PlayOneShot(m_deathSFX);

                // Death behaviour.
                m_GameOverUI.SetActive(true);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator IFrames(float length)
    {
        m_canTakeDamage = false;

        yield return new WaitForSeconds(length);

        m_canTakeDamage = true;
    }

    void Heal(int increase)
    {
        m_currentHealth += increase;
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);

        m_audioSource.PlayOneShot(m_healSFX);

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    // Increase players max health and current health by param.
    public void AddMaxHealth(int maxIncrease)
    {
        m_maxHealth += maxIncrease;

        // Ensure healing value is multiple of 2.
        if (m_maxHealth % 2 != 0)
        {
            m_maxHealth++;
            m_currentHealth += maxIncrease * 2;
        }
        else
        {
            m_currentHealth += maxIncrease;
        }

        // Clamp Values.
        m_maxHealth = Mathf.Clamp(m_maxHealth, 1, 16);
        m_currentHealth = Mathf.Clamp(m_currentHealth, 1, 16);

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }

    // Increase players max health by maxIncrease and heals currentHealth by currentIncrease.
    public void AddMaxHealth(int maxIncrease, int currentIncrease)
    {
        m_maxHealth += maxIncrease;

        // Ensure value is multiple of 2.
        if (m_maxHealth % 2 != 0)
        {
            m_maxHealth++;
        }

        m_currentHealth += currentIncrease;

        m_healthScript.UpdateHealthUI(m_currentHealth, m_maxHealth);
    }
    #endregion

    #region SFX

    // Play random fx from sfx parameter array.
    void PlayRandomHurtSFX(AudioClip[] audioArray)
    {
        int randomIndex = Random.Range(0, audioArray.Length - 1);

        m_audioSource.PlayOneShot(audioArray[randomIndex]);
    }

    #endregion
}
