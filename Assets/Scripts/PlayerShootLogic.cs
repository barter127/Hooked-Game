using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerShootLogic : MonoBehaviour
{
    /// <summary>
    ///  Handles the rope shoot logic
    ///  Turns of rope and knife rendering
    ///  Initialises Rope health UI and uses it's methods
    /// </summary>

    #region Variables
    // Player Transform.
    [SerializeField] Transform m_playerTrans;

    [Header("Knife Fire")]
    // Point projectile spawns from.
    [SerializeField] Transform m_firePointTrans;

    InputAction m_attackAction;

    // Force applied when firing
    [SerializeField] float m_fireForce;
    [SerializeField] int m_amountOfForcesToApply;

    // Rope Health Bar References
    [SerializeField] GameObject m_ropeHealthPanel;
    [SerializeField] RopeHealthLogic m_ropeHealthLogic;
    int m_forcesLeft;

    [Header("Knife Return")]

    // A list of all the rope renderers.
    List<SpriteRenderer> m_allRopeRenderers = new List<SpriteRenderer>();

    // Knife Prefab.
    [SerializeField] GameObject m_knifeReference;
    [SerializeField] GameObject m_ropeReference;

    // Knife Components
    Rigidbody2D m_knifeRb;
    SpriteRenderer m_knifeRenderer;
    KnifeFollowMouse m_knifeFollowMouse;
    KnifeEnemyAttachLogic m_knifeAttachLogic;

    // Speed knife returns to player.
    [SerializeField] float m_returnSpeed;
    // Distance needed to return knife.
    [SerializeField] float m_returnMagnitude;

    public static bool m_hasFired { get; private set; } = false;

    bool m_isReturning;

    [Header("Rope Damage")]
    float m_ropeHealth;
    [SerializeField] float m_ropeMaxHealth;

    // Distance where rope starts taking damage.
    [SerializeField] float m_ropeDamageDistance;

    bool m_tickCountdownStarted;
    #endregion

    void Start()
    {
        // Get all rope renderers GOs.
        GameObject[] ropeObjects = GameObject.FindGameObjectsWithTag("Rope");

        // Get the SpriteRenderers and add to the list.
        foreach (GameObject go in ropeObjects)
        {
            m_allRopeRenderers.Add(go.GetComponent<SpriteRenderer>());
        }

        m_ropeHealth = m_ropeMaxHealth;

        // Get components.
        m_knifeRb = m_knifeReference.GetComponent<Rigidbody2D>();
        m_knifeRenderer = m_knifeReference.GetComponent<SpriteRenderer>();

        // Get scripts.
        m_knifeFollowMouse = m_knifeReference.GetComponent<KnifeFollowMouse>();
        m_knifeAttachLogic = m_knifeReference.GetComponent<KnifeEnemyAttachLogic>();

        // Initialise rope vars and appearance.
        m_hasFired = false;
        m_isReturning = true;

        SetRopeSpriteRenderer(false);
    }

    private void Update()
    {
        // Only check if rope should be damaged when attached.
        if (m_knifeAttachLogic.m_isConnected)
        {
            CheckRopeDamage();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_isReturning)
        {
            ReturnKnife();
        }

        // Check if forces should still be applied
        if (m_forcesLeft > 0)
        {
            // Get direction bullet needs to travel.
            Vector3 bulletDir = (m_firePointTrans.position - m_playerTrans.position).normalized;

            m_knifeRb.AddForce(bulletDir * m_fireForce, ForceMode2D.Impulse);

            m_forcesLeft--;
        }

        // When not visible move knifeRb with the player.
        if (m_knifeRb.bodyType == RigidbodyType2D.Kinematic)
        {
            m_knifeRb.MovePosition(m_playerTrans.position);
        }
    }

    #region Handle Input Sys

    void OnEnable()
    {
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_attackAction.performed += Attack;
        m_attackAction.Enable();

    }

    void OnDisable()
    {
        m_attackAction.performed -= Attack;
        m_attackAction.Disable();
    }

    #endregion

    // Attack Button Pressed.
    void Attack(InputAction.CallbackContext context)
    {
        if (!m_hasFired)
        {
            Debug.Log("HI");

            FireKnife();
        }
        else
        {
            Debug.Log("Return");
            // Prevents recalling the knife if attached to a wall or enemy.
            if (!m_knifeAttachLogic.m_isConnected)
            {
                Debug.Log("Actual return");

                StartKnifeReturn();
            }
        }
    }

    // On click spawn knife and fire in mouse direction.
    void FireKnife()
    {
        m_hasFired = true;

        // Get direction bullet needs to travel.
        Vector3 bulletDir = (m_firePointTrans.position - m_playerTrans.position).normalized;

        // Enable existing knife object.
        SetRopeSpriteRenderer(true);
        m_knifeAttachLogic.enabled = true;

        m_knifeRb.bodyType = RigidbodyType2D.Dynamic;

        // Fire projectile in direction. Rotated to face direction.
        m_knifeRb.linearVelocity = Vector3.zero; // Reset velocity to ensure accurate force application.
        m_forcesLeft = m_amountOfForcesToApply;

        m_ropeHealthPanel.SetActive(true);
    }

    public void StartKnifeReturn()
    {
        m_isReturning = true;

        // Disable Knife Logic. Maybe this could be a method?
        m_knifeAttachLogic.enabled = false;
        m_knifeFollowMouse.enabled = false;

        // Ensure knife values are correct.
        m_knifeAttachLogic.DetatchEnemy();

        m_ropeHealthPanel.SetActive(false);
    }

    // Move towards player. Delete at destination.
    void ReturnKnife()
    {
        // Get direction bullet needs to travel.
        Vector3 bulletDir = (m_playerTrans.position - m_firePointTrans.position).normalized;

        float angle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg + 90; // Arbitrary 90, fixes incorrect rotation.

        // Knife hit destination
        if (Vector3.Distance(m_knifeReference.transform.position, m_playerTrans.position) <= m_returnMagnitude)
        {
            m_knifeRb.bodyType = RigidbodyType2D.Kinematic;
            SetRopeSpriteRenderer(false);

            m_isReturning = false;
            m_hasFired = false;
        }
        else
        {
            // Use Rigidbody2D to move knife;
            Vector2 direction = (m_playerTrans.position - m_knifeReference.transform.position).normalized;

            if (m_knifeRb.bodyType == RigidbodyType2D.Dynamic)
            {
                m_knifeRb.linearVelocity = direction * m_returnSpeed;
            }
            
        }

    }

    // If player is too far from knife apply rope damage based on distance or intensity of forces
    // Return Knife if rope health is 0.
    // Alot of nesting but I don't think it's excessive.
    void CheckRopeDamage()
    {
        // Distance between player and knife.
        float distance = Vector3.Distance(m_playerTrans.position, m_knifeReference.transform.position);
        if (distance > m_ropeDamageDistance)
        {
            if (!m_tickCountdownStarted)
            {
                StartCoroutine(RopeDamageTick());
            }
        }

        // Check is not nessecary but adds clarity of intention.
        else if (distance < m_ropeDamageDistance) 
        {
            // Cancel tick timer.
            if (m_tickCountdownStarted)
            {
                StopCoroutine(RopeDamageTick());
            }
        }
    }

    // Apply damage to the rope
    void ApplyRopeDamage()
    {
        m_ropeHealth -= 2;
        Debug.Log(m_ropeHealth); // ADD UI!!!!

        if (m_ropeHealth <= 0)
        {
            // Sometimes unessecary but occasionally rb can be static.
            m_knifeRb.bodyType = RigidbodyType2D.Dynamic;

            StartKnifeReturn();

            m_ropeHealth = m_ropeMaxHealth;
        }
    }

    // Times damage
    IEnumerator RopeDamageTick()
    {
        m_tickCountdownStarted = true;

        yield return new WaitForSeconds(1f);

        ApplyRopeDamage();

        m_tickCountdownStarted = false;
    }

    // Function is limited in usability but makes overall code more readable.
    void SetRopeSpriteRenderer(bool isEnabled)
    {
        m_knifeRenderer.enabled = isEnabled;

        foreach (SpriteRenderer spr in m_allRopeRenderers)
        {
            spr.enabled = isEnabled;
        }
    }
}
