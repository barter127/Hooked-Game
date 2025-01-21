using UnityEngine;

public class KnifeEnemyAttachLogic : MonoBehaviour
{
    /// <summary>
    /// Attach enemy collided with to distance joint on knife object.
    /// Enable dj when nessecary disable when not connected.
    /// 
    /// Handles disconnect logic when too much strain put on rope.
    /// </summary>

    bool isConnected = false;

    // Distance joint on self.
    DistanceJoint2D m_distanceJoint;
    Rigidbody2D m_rigidbody;

    // VFX for hit confirmation.
    [SerializeField] GameObject m_hitFX;
    [SerializeField] Vector3 m_spawnOffset;

    void Start()
    {
        m_distanceJoint = GetComponent<DistanceJoint2D>();
        m_rigidbody = GetComponent<Rigidbody2D>();

        m_distanceJoint.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verify collider was an enemy.
        if (collision.CompareTag("Enemy") && !isConnected)
        {
            isConnected = true;

            Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Instantiate FX. Hit FX should follow knife.
            Vector3 spawnPos = transform.position + m_spawnOffset;
            GameObject spawnedHitFX = Instantiate(m_hitFX, spawnPos, Quaternion.identity);

            // Set follow transform.
            FollowTransform follow = spawnedHitFX.GetComponent<FollowTransform>();
            follow.m_transformToFollow = transform;


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

        // If collided with obstacle.
        else if (collision.CompareTag("Obstacle"))
        {
            // Stick to obstacle.
            m_rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}
