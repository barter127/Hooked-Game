using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootLogic : MonoBehaviour
{
    [SerializeField] GameObject m_projectile;
    [SerializeField] Transform m_player;

    bool m_hasFired;
    GameObject m_knifeReference;

    private InputAction m_attackAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {

        if (m_attackAction.IsPressed() && !m_hasFired)
        {
            if (!m_hasFired)
            {
                Fire();
                m_hasFired = true;
            }
            else
            {
                //m_knifeReference.GetComponent<Rigidbody2D>().AddForce();
            }
        }

        // Rotate weapon around Player.
        transform.RotateAround(m_player.position, Vector3.forward, 100 * Time.deltaTime);

        
    }

    void Fire()
    {
        // Get direction bullet needs to travel.
        Vector3 bulletDir = (transform.position - m_player.position).normalized;

        float radvalue = Mathf.Atan2(bulletDir.y, bulletDir.x);
        float angle = radvalue * (180 / Mathf.PI);

        // Fire projectile in direction. !! UPDATE FOR OBJECT POOLING !!
        m_knifeReference = Instantiate(m_projectile, transform.position, Quaternion.identity);
        m_knifeReference.transform.Rotate(Vector3.forward, angle - 90); // 90 is an arbitrary value to fix offset.
        m_knifeReference.GetComponent<Rigidbody2D>().AddForce(bulletDir * 10, ForceMode2D.Impulse);
    }
}
