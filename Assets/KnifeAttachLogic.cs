using UnityEngine;

public class KnifeAttachLogic : MonoBehaviour
{
    /// <summary>
    /// Attach enemy collided with to distance joint on knife object.
    /// Enable dj when nessecary disable when not connected.
    /// 
    /// Handles disconnect logic when too much strain put on rope.
    /// </summary>

    // Distance joint on self.
    DistanceJoint2D m_distanceJoint;

    void Start()
    {
        m_distanceJoint = GetComponent<DistanceJoint2D>();

        m_distanceJoint.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verify collider was an enemy.
        if (collision.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Validate rb.
            if (enemyRb != null)
            {
                // Attach enemy rb to distance joint.
                m_distanceJoint.enabled = true;
                m_distanceJoint.connectedBody = enemyRb;
            }
            else
            {
                Debug.Log("Enemy does not have rb attached.");
            }
        }
    }
}
