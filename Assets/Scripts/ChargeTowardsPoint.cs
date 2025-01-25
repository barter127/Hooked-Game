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

    float m_chargeTimer;

    // Components
    Rigidbody2D m_rigidbody;
    SpriteRenderer m_spriteRenderer;

    Vector3 m_targetVector;

    public bool m_startCharge = false; // For animator.
    bool m_isFacingRight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check prevents errors after player/object is destoryed.
        if (m_targetTransform != null)
        {
            m_targetVector = m_targetTransform.position - transform.position;
        }

        DrawLOS(m_targetVector);

        // In range
        if (m_targetVector.magnitude < m_chargeRange)
        {
            m_chargeTimer -= Time.deltaTime;

            CheckDirectionToFace(m_targetVector.x < 0);

            // Charge timer up.
            if (m_chargeTimer <= 0) 
            {
                // If has line of sight.
                RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetVector, m_targetVector.magnitude, m_obstacleLayer);
                if (hit.collider == null)
                {
                    Charge();
                }
            }
        }

        else if (m_chargeTimer != m_rateOfCharges)
        {
            m_chargeTimer = m_rateOfCharges;
        }
    }

    void Charge()
    {
        m_startCharge = true; // Set false in animation script.

        m_chargeTimer = m_rateOfCharges;

        m_targetVector = m_targetTransform.position - transform.position;
        m_rigidbody.AddForce(m_targetVector * m_chargeForce);
    }

    // Suboptimal as resuses logic but is self contained.
    void DrawLOS(Vector3 targetPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos, targetPos.magnitude, m_obstacleLayer);

        // In range.
        if (targetPos.magnitude < m_chargeRange)
        {
            // Has Line of Sight.
            if (hit.collider == null)
            {
                Debug.DrawRay(transform.position, targetPos, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, targetPos, Color.red);
            }
        }
    }

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
}
