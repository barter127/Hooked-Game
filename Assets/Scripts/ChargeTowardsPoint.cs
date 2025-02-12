using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
public class ChargeTowardsPoint : MonoBehaviour
{
    /// <summary>
    /// Perodically charge towards targetTransform.
    /// Only charge when chargeTimer is up.
    /// Only charge when in range.
    /// </summary>

    [Header ("Charge Variables")]
    [SerializeField] Transform m_targetTransform;
    // Force of charge.
    [SerializeField] float m_chargeForce;
    // Time between each charge in seconds.
    [SerializeField] float m_rateOfCharges;
    // Furthest distance can charge from.
    [SerializeField] float m_chargeRange;
    // Layer with obstacles.
    [SerializeField] LayerMask m_obstacleLayer;

    [SerializeField] float m_attachedForceMultiplier;

    float m_chargeTimer;

    // Components
    Rigidbody2D m_rigidbody;
    SpriteRenderer m_spriteRenderer;
    StateMachine m_stateMachine;

    Vector3 m_targetVector;

    [HideInInspector] public bool m_startCharge = false; // For animator.
    bool m_isFacingRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // If no default target set to player.
        if (m_targetTransform == null)
        {
            m_targetTransform = TransformReferenceHolder.m_player.transform;
        }

        m_rigidbody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_stateMachine = GetComponent<StateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check prevents errors after player/object is destoryed.
        if (m_targetTransform != null)
        {
            m_targetVector = m_targetTransform.position - transform.position;
        }

        // In range
        if (m_targetVector.magnitude < m_chargeRange && m_stateMachine.m_currentState == StateMachine.AIState.Idle)
        {
            // Called every frame but statemachine ignores most requests.
            m_stateMachine.ChangeState(StateMachine.AIState.Moving);
        }

        #region State Machine Checks

        if (m_stateMachine.m_currentState == StateMachine.AIState.Moving)
        {
            m_chargeTimer -= Time.deltaTime;

            CheckDirectionToFace(m_targetVector.x < 0);

            // Charges if clear LOS, Resets Timer.
            CheckChargeLOS(1, true);
        }

        else if (m_stateMachine.m_currentState == StateMachine.AIState.Attached)
        {
            m_chargeTimer -= Time.deltaTime;

            // Charges if clear LOS, Resets Timer.
            CheckChargeLOS(m_attachedForceMultiplier ,false);
        }

        else if (m_chargeTimer != m_rateOfCharges)
        {
            m_stateMachine.ChangeState(StateMachine.AIState.Idle);
            m_chargeTimer = m_rateOfCharges;
        }

        #endregion
    }

    #region Charging

    void CheckChargeLOS(float chargeMultiplier, bool chargeTowards)
    {
        // Charge timer up.
        if (m_chargeTimer <= 0)
        {
            // If has line of sight.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetVector, m_targetVector.magnitude, m_obstacleLayer);
            if (hit.collider == null)
            {
                Charge(chargeMultiplier, chargeTowards);
            }
        }
    }

    void Charge(float chargeMultiplier, bool chargeTowards)
    {
        m_startCharge = true; // Set false in animation script.

        // Reset Timer
        m_chargeTimer = m_rateOfCharges;

        // Get target direction (with magnitude)
        m_targetVector = m_targetTransform.position - transform.position;

        if (!chargeTowards)
        {
            // Flip Target Vector. (charges away)
            m_targetVector *= -1;
        }

        m_rigidbody.AddForce(m_targetVector * m_chargeForce * chargeMultiplier);
    }

    #endregion

    #region Direction to Face
    void Turn()
    {
        m_isFacingRight = !m_isFacingRight;
        m_spriteRenderer.flipX = m_isFacingRight;
    }

    void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != m_isFacingRight)
        {
            Turn();
        }
    }
    #endregion
}
