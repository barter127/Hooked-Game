using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector3 bulletDir;
    [SerializeField] float shotSpeed;
    public float damage;

    // Multiplier for bullet momentum.
    [SerializeField] float momentumPos;
    [SerializeField] float momentumNeg;

    private void FixedUpdate()
    {
        transform.position += bulletDir * shotSpeed * Time.fixedDeltaTime;
    }

    public void SetDirection(Vector3 newBulletDir)
    {
        bulletDir = newBulletDir;
    }

    public void SetDirection(Vector2 newBulletDir)
    {

        bulletDir = new Vector3(newBulletDir.x, newBulletDir.y, 0);
    }
}
