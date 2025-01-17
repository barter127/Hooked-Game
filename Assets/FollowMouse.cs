using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody2D rb;
    Transform player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Character").transform;
    }

    void Update()
    {
        // Knife hit destination.
        if (Vector3.Distance(transform.position, player.transform.position) > 0.5)
        {
            // Get mouse position in world space.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // Ensure the z-coordinate is 0.

            // Calculate the direction from the current position to the mouse position.
            Vector3 direction = mousePosition - transform.position;
            direction.Normalize();

            // Apply force in the direction of the mouse.
            rb.AddForce(direction * speed);
        }


    }
}
