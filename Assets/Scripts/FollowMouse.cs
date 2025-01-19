using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] float m_speed = 5f;

    Rigidbody2D m_rigidbody;
    [SerializeField] Transform m_playerTrans;
    Vector3 m_mousePosition;

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
            m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_mousePosition.z = 0; // Ensure the z-coordinate is 0.

            Debug.Log("In range");
        }
    }

    private void FixedUpdate()
    {
        // Calculate the direction from the current position to the mouse position.
        Vector3 direction = m_mousePosition - transform.position;
        direction.Normalize();

        // Apply force in the direction of the mouse.
        m_rigidbody.AddForce(direction * m_speed);
    }
}
