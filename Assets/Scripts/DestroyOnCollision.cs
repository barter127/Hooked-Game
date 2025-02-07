using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") || collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
