using UnityEngine;

public class KnifeFollowMouse : MonoBehaviour
{
    /// <summary>
    /// Rigidbody movement to get the knife towards the mouse.
    /// Stop moving within acceptable range.
    /// Change acceleration based on mouse distance.
    /// </summary>

    [SerializeField] float m_knifeMaxSpeed;
    [SerializeField] float m_knifeAccelRate;

    public Rigidbody2D m_rigidbody;
    [SerializeField] Transform m_playerTrans;
    Vector3 m_mousePosition;

    // Only apply forces every X seconds.
    [SerializeField] float m_moveTimerLength;
    float m_moveTimer;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get mouse position in world space. Could be better optimised.
        m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m_mousePosition.z = 0; // Ensure the z-coordinate is 0.
    }

    private void FixedUpdate()
    {
        if (m_moveTimer <= Time.time)
        {
            m_moveTimer = Time.time + m_moveTimerLength;

            // Calculate the direction from the current position to the mouse position.
            Vector3 direction = m_mousePosition - transform.position;
            direction.Normalize();

            float xTarget = direction.x * m_knifeMaxSpeed;
            float xSpeedDif = xTarget - m_rigidbody.linearVelocityX;
            float xMovement = xSpeedDif * m_knifeAccelRate;

            float yTarget = direction.y * m_knifeMaxSpeed;
            float ySpeedDif = yTarget - m_rigidbody.linearVelocity.y;
            float yMovement = ySpeedDif * m_knifeAccelRate;



            // Apply force in the direction of the mouse.
            m_rigidbody.AddForce(xMovement * Vector2.right, ForceMode2D.Force);
            m_rigidbody.AddForce(yMovement * Vector2.up, ForceMode2D.Force);
        }
    }
}
