using UnityEngine;

/// <summary>
/// Move in a random direction each random direction of time.
/// Clamp to minDistance so movement isn't negligible.
/// </summary>

[RequireComponent (typeof(Rigidbody2D))]
public class RandomisedMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D m_rigidbody;
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] bool m_startCharge; // For animator.
    bool m_isFacingRight;

    [SerializeField, Range(0, 10)] float m_minDistance;
    [SerializeField, Range(0, 10)] float m_maxDistance;

    // Time between attacks;
    [SerializeField, Range(0, 10)] float m_minTime; //Arbitrary large value. 
    [SerializeField, Range(0, 10)] float m_maxTime; //Arbitrary large value. 

    // Randomised values.
    Vector2 movementDir;
    float nextMovement;

    private void Start()
    {
        m_isFacingRight = true;
    }

    private void Update()
    {
        if (nextMovement < 0)
        {
            m_startCharge = true;

            // Randomise timer.
            nextMovement = Random.Range(m_minTime, m_maxTime);

            // Randomise movementDir.
            movementDir.x = Random.Range(-m_maxDistance, m_maxDistance);
            movementDir.y = Random.Range(-m_maxDistance, m_maxDistance);

            // Clamp direction to not exceed set min and max.
            movementDir.x = ClampDirection(movementDir.x);
            movementDir.y = ClampDirection(movementDir.y);

            CheckDirectionToFace(movementDir.x > 0);
            m_rigidbody.AddForce(movementDir, ForceMode2D.Impulse);
        }
        else nextMovement -= Time.deltaTime;
    }

    float ClampDirection(float distance)
    {
        if (distance > 0)
        {
            return Mathf.Clamp(distance, m_minDistance, m_maxDistance);
        }
        else
        {
            return Mathf.Clamp(distance, -m_maxDistance, -m_minDistance);
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
