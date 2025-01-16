using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z-coordinate is 0

        // Calculate the direction from the current position to the mouse position
        Vector3 direction = mousePosition - transform.position;
        direction.Normalize();

        // Apply force in the direction of the mouse
        GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }
}
