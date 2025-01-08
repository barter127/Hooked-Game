using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Vector3 m_bulletDir;
    [SerializeField] float m_shotSpeed;
    public float m_damage { get; private set; }

    private void FixedUpdate()
    {
        transform.position += m_bulletDir * m_shotSpeed * Time.fixedDeltaTime;
    }

    public void SetDirection(Vector3 newBulletDir)
    {
        m_bulletDir = newBulletDir;
    }

    public void SetDirection(Vector2 newBulletDir)
    {

        m_bulletDir = new Vector3(newBulletDir.x, newBulletDir.y, 0);
    }
}
