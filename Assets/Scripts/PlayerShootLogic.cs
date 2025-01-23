using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerShootLogic : MonoBehaviour
{
    #region Variables
    // Player Transform.
    [SerializeField] Transform m_playerTrans;

    // Point projectile spawns from.
    [SerializeField] Transform m_firePointTrans;

    InputAction m_attackAction;

    [Header ("Knife Return")]

    // Knife Prefab.
    [SerializeField] GameObject m_knifeReference;
    [SerializeField] GameObject m_ropeReference;

    Rigidbody2D m_knifeRb;
    HingeJoint2D m_hingeJoint; // I LOVE UNITY!!!!!!!
    KnifeFollowMouse m_knifeFollowMouse;
    KnifeEnemyAttachLogic m_knifeAttachLogic;

    // Speed knife returns to player.
    [SerializeField] float m_returnSpeed;
    // Distance needed to return knife.
    [SerializeField] float m_returnMagnitude;

    // Maybe switch to get private set this is SCARY.
    public static bool m_hasFired = false;
    public bool m_isReturning;

    [Header("Rope Damage")]
    [SerializeField] float m_ropeHealth;

    // Distance where rope starts taking damage.
    [SerializeField] float m_ropeDamageDistance;

    bool m_tickCountdownStarted;
    #endregion

    void Start()
    {
        // Knife object starts loaded.
        m_hasFired = true;

        // Return knife on start (prolly covered by fade in).
        m_isReturning = true;

        // Get components.
        m_knifeRb = m_knifeReference.GetComponent<Rigidbody2D>();
        m_hingeJoint = m_knifeReference.GetComponent<HingeJoint2D>();

        // Get scripts.
        m_knifeFollowMouse = m_knifeReference.GetComponent<KnifeFollowMouse>();
        m_knifeAttachLogic = m_knifeReference.GetComponent<KnifeEnemyAttachLogic>();
    }

    private void Update()
    {
        CheckRopeDamage();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_isReturning)
        {
            ReturnKnife();
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

    // Rotate cursor around player and point to current mouse position.
    void RotateToMouse()
    {
        // Read mouse input. Use world position for extreme precision.
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // Calculate distance from mouse (target for currentPos).
        Vector3 distanceFromMouse = mouseWorldPos - m_playerTrans.position;

        // Find the position of the crosshair.
        Vector3 currentPos = transform.position - m_playerTrans.position;

        // Check if crosshair still needs to move to reach desired location.
        if (currentPos != distanceFromMouse)
        {
            Vector3 direction = mouseWorldPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180; // Arbritrary 180 fixes incorrect rotation.

            //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    // Attack Button Pressed.
    void Attack(InputAction.CallbackContext context)
    {
        if (!m_hasFired)
        {
            FireKnife();
        }
        else
        {
            m_isReturning = true;
            
            // Disable Knife Logic. Maybe this could be a method?
            m_knifeAttachLogic.enabled = false;
            m_knifeFollowMouse.enabled = false;
        }
    }

    // On click spawn knife and fire in mouse direction.
    void FireKnife()
    {
        m_hasFired = true;

        // Get direction bullet needs to travel.
        Vector3 bulletDir = (m_firePointTrans.position - m_playerTrans.position).normalized;

        // Enable existing knife object.
        m_knifeReference.SetActive(true);
        m_ropeReference.SetActive(true);

        // Fixes what I belive to be a bug with hinge joints.
        m_hingeJoint.enabled = false;
        m_hingeJoint.enabled = true;

        // Fire projectile in direction. Rotated to face direction.
        m_knifeRb.linearVelocity = Vector3.zero; // Reset velocity to ensure accurate force application.
        m_knifeRb.AddForce(bulletDir * 30, ForceMode2D.Impulse);
    }

    public void StartKnifeReturn()
    {
        m_isReturning = true;

        // Disable Knife Logic. Maybe this could be a method?
        m_knifeAttachLogic.enabled = false;
        m_knifeFollowMouse.enabled = false;
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
            
            m_knifeReference.SetActive(false);
            m_ropeReference.SetActive(false);

            m_isReturning = false;
            m_hasFired = false;
        }
        else
        {
            // Use Rigidbody2D to move knife
            m_knifeRb = m_knifeReference.GetComponent<Rigidbody2D>();
            Vector2 direction = (m_playerTrans.position - m_knifeReference.transform.position).normalized;
            m_knifeRb.linearVelocity = direction * m_returnSpeed;
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
            m_knifeAttachLogic.m_isConnected = false;
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
}
