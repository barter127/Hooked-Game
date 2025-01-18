using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] float m_speed = 5f;

    Rigidbody2D m_rigidbody;
    [SerializeField] Transform m_playerTrans;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_playerTrans = GameObject.Find("Character").transform;
    }

    void Update()
    {
        // Knife hit destination.
        if (Vector3.Distance(transform.position, m_playerTrans.transform.position) > 0.5)
        {
            // Get mouse position in world space.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the z-coordinate is 0.

            // Calculate the direction from the current position to the mouse position.
            Vector3 direction = mousePosition - transform.position;
            direction.Normalize();

            // Apply force in the direction of the mouse.
            m_rigidbody.AddForce(direction * m_speed);
        }


    }
}
