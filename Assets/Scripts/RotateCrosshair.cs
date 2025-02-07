using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCrosshair : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector2 distance;
    Vector2 mouseWorldPos;

    void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // Calculate distance from crosshair to mouse position.
        distance = new Vector2(transform.position.x - mouseWorldPos.x, transform.position.y - mouseWorldPos.y);

        // Find position of the crosshair relative to the player.
        Vector2 currentPos = new Vector2(transform.position.x - player.position.x, transform.position.y - player.position.y);

        // Check if the current position is different from the target distance.
        if (currentPos != distance)
        {
            Vector2 direction = mouseWorldPos - (Vector2)player.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
        }
    }
}
