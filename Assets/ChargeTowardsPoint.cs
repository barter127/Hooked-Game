using UnityEngine;

/// <summary>
/// Perodically charge towards targetTransform.
/// Only charge when chargeTimer is up.
/// Only charge when in range.
/// </summary>

[RequireComponent (typeof(Rigidbody2D))]
public class ChargeTowardsPoint : MonoBehaviour
{
    [SerializeField] Transform m_targetTransform;
    // Force of charge.
    [SerializeField] float m_chargeForce;
    // Time between each charge in seconds.
    [SerializeField] float m_rateOfCharges;
    // Furthest distance can charge from.
    [SerializeField] float m_chargeRange;

    float m_chargeTimer;

    Rigidbody2D m_rigidbody;
    [SerializeField] LayerMask m_obstacleLayer;
    Vector3 m_targetVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_targetVector = m_targetTransform.position - transform.position;

        DrawLOS(m_targetVector);

        // Can charge.
        if (m_chargeTimer <= 0 && m_targetVector.magnitude < m_chargeRange)
        {
            // If has line of sight.
            RaycastHit2D hit = Physics2D.Raycast(transform.position, m_targetVector, m_targetVector.magnitude, m_obstacleLayer);
            if (hit.collider == null)
            {
                Charge();
            }
        }
        else m_chargeTimer -= Time.deltaTime;
    }

    void Charge()
    {
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
}
