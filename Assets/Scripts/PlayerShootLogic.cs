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
                m_hasFired = true;
            }
            else
            {

                Debug.Log("HI!!!");
                Vector3 knifeDirect = (m_knifeReference.transform.position - transform.position).normalized;
                m_knifeReference.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                m_knifeReference.GetComponent<Rigidbody2D>().AddForce(knifeDirect * 10, ForceMode2D.Impulse);
            }
        }



        // Rotate weapon around Player.
        m_mousePosition = m_lookAction.ReadValue<Vector2>();
        transform.RotateAround(m_playerTrans.position, Vector3.forward, 100 * Time.deltaTime);

        
    }

    void Fire()
    {
        // Get direction bullet needs to travel.
        Vector3 bulletDir = (transform.position - m_playerTrans.position).normalized;

        float radvalue = Mathf.Atan2(bulletDir.y, bulletDir.x);
        float angle = radvalue * (180 / Mathf.PI);

        // Fire projectile in direction. !! UPDATE FOR OBJECT POOLING !!
        m_knifeReference = Instantiate(m_projectile, transform.position, Quaternion.identity);
        m_knifeReference.transform.Rotate(Vector3.forward, angle - 90); // 90 is an arbitrary value to fix offset.
        m_knifeReference.GetComponent<Rigidbody2D>().AddForce(bulletDir * 10, ForceMode2D.Impulse);
    }
}
