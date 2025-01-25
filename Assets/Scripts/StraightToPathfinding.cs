using UnityEngine;

/// <summary>
/// Move straight towards target object at consistent rate.
/// Doesn't consider obstacles.
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
public class StraightToPathfinding : MonoBehaviour
{
    [SerializeField] Transform m_targetTransform;
    [SerializeField] float m_speed;

    bool m_inView = false;

    void FixedUpdate()
    {
        if (m_inView)
        {
            // Move at consistent rate towards target.
            transform.position = Vector2.MoveTowards(transform.position, m_targetTransform.position, m_speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        m_inView = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        m_inView = false;
    }
}
