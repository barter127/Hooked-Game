using UnityEngine;

public class BulletPatternSpawner : MonoBehaviour
{
    [SerializeField] GameObject m_bullet;
    [SerializeField] float m_angleScale;
    [SerializeField] float m_fireRate;
    [SerializeField] float m_bulletsPerShot;
    [SerializeField] int m_offsetIncrease;

    float m_rotationAngle = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("Spiral", 0f, m_fireRate);
        m_angleScale = 360 / m_bulletsPerShot;
    }

    void Update()
    {
        m_angleScale = 360 / m_bulletsPerShot;
    }

    void Spiral()
    {
        Vector3 bulletDir = Vector3.zero;

        // Loop for amount of shots.
        for (int i = 0; i <= m_bulletsPerShot - 1; i++)
        {
            // Get current angle based off iter.
            float degrees = m_rotationAngle + m_angleScale * i;

            // Get bullet move dir.
            float bulletDirX = Mathf.Sin((degrees * Mathf.PI) / 180);
            float bulletDirY = Mathf.Cos((degrees * Mathf.PI) / 180);

            bulletDir = new Vector3(bulletDirX, bulletDirY, 0);

            // Spawn bullet and set direction.
            GameObject spawnedBull = Instantiate(m_bullet, transform.position, transform.rotation);
            spawnedBull.GetComponent<BulletMovement>().SetDirection(bulletDir);
        }

        // Update rotation angle.
        m_rotationAngle += m_offsetIncrease;
        if (m_rotationAngle >= 360)
        {
            m_rotationAngle = 0;
        }
    }

    void Straight()
    {
        Vector3 bulletDir = Vector3.zero;

        for (int i = 0; i <= m_bulletsPerShot - 1; i++)
        {
            // Get current angle based off iter.
            float shotAngle = m_angleScale * i;

            // Calculate the bullet's direction using trigonometry
            bulletDir.x = Mathf.Cos(shotAngle * Mathf.PI / 180);
            bulletDir.y = Mathf.Sin(shotAngle * Mathf.PI / 180);

            GameObject spawnedBull = Instantiate(m_bullet, transform.position, transform.rotation);
            spawnedBull.GetComponent<BulletMovement>().SetDirection(bulletDir);
        }
    }

    void Scattershot()
    {
        Vector3 bulletDir = Vector3.zero;

        for (int i = 0; i <= m_bulletsPerShot - 1; i++)
        {
            // Get current angle based off iter.
            float rotationAngle = m_angleScale * i;
            rotationAngle = Mathf.Deg2Rad * rotationAngle;

            // Calculate the bullet's direction using trigonometry
            // Vector up unnecessary but adds readability.
            bulletDir.x = Mathf.Cos(Vector2.up.x * rotationAngle) - Mathf.Sin(Vector2.up.y * rotationAngle);
            bulletDir.y = Mathf.Sin(Vector2.up.x * rotationAngle) + Mathf.Cos(Vector2.up.y * rotationAngle);
            bulletDir = bulletDir.normalized;

            GameObject spawnedBull = Instantiate(m_bullet, transform.position, transform.rotation);
            spawnedBull.GetComponent<BulletMovement>().SetDirection(bulletDir);
        }
    }

}
