using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootLogic : MonoBehaviour
{
    [SerializeField] GameObject m_projectile;
    [SerializeField] Transform m_playerTrans;

    InputAction m_lookAction;
    Vector2 m_mousePosition;

    bool m_hasFired;
    GameObject m_knifeReference;

    private InputAction m_attackAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {

        if (m_attackAction.IsPressed())
        {
            if (!m_hasFired)
            {
                Fire();
                //m_hasFired = true;
            }
            else
            {
                Debug.Log("HI!!!");
                Vector3 knifeDirect = (m_knifeReference.transform.position - transform.position).normalized;
                m_knifeReference.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                m_knifeReference.GetComponent<Rigidbody2D>().AddForce(knifeDirect * 10, ForceMode2D.Impulse);
            }
        }

        // Read mouse input. Use world position for extreme precision.
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // Calculate distance from the player to the mouse position.
        Vector3 distanceFromMouse = mouseWorldPos - m_playerTrans.position;

        // Find the position of the crosshair relative to the player.
        Vector3 currentPos = transform.position - m_playerTrans.position;

        // Check crosshair still needs to move to reach desired location.
        if (currentPos != distanceFromMouse)
        {
            Vector3 direction = mouseWorldPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180; // Arbritrary 180 fixes incorrect rotation.

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

    }

    void Fire()
    {
        // Get direction bullet needs to travel.
        Vector3 bulletDir = (transform.position - m_playerTrans.position).normalized;

        // Find rotation for spawned bullet.
        float radvalue = Mathf.Atan2(bulletDir.y, bulletDir.x);
        float angle = radvalue * (180 / Mathf.PI);

        // Fire projectile in direction. Rotated to face direction.
        m_knifeReference = Instantiate(m_projectile, transform.position, Quaternion.identity);
        m_knifeReference.transform.Rotate(Vector3.forward, angle + 180); // 180 is an arbitrary value to fix offset.
        m_knifeReference.GetComponent<Rigidbody2D>().AddForce(bulletDir * 10, ForceMode2D.Impulse);
    }
}
