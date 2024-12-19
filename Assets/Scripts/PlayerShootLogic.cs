using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootLogic : MonoBehaviour
{
    // Knife Prefab.
    [SerializeField] GameObject m_knifeObject;
    // Player Transform.
    [SerializeField] Transform m_playerTrans;

    // Point projectile spawns from.
    [SerializeField] Transform m_firePointTrans;

    [Header ("Knife Return")]

    // Speed knife returns to player.
    [SerializeField] float m_returnSpeed;
    // Distance needed to destroy knife.
    [SerializeField] float m_returnMagnitude;

    bool m_hasFired;
    bool m_isReturning;
    GameObject m_knifeReference;

    private InputAction m_attackAction;



    #region Handle Input Sys

    private void OnEnable()
    {
        m_attackAction = InputSystem.actions.FindAction("Attack");
        m_attackAction.performed += Attack;
        m_attackAction.Enable();
        
    }

    private void OnDisable()
    {
        m_attackAction.performed -= Attack;
        m_attackAction.Disable();
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        RotateToMouse();


        if (m_isReturning)
        {
            ReturnKnife();
        }
    }

    // Rotate cursor around player and point to current mouse position.
    void RotateToMouse()
    {
        // Read mouse input. Use world position for extreme precision.
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        // Calculate distance from mouse (target for currentPos).
        Vector3 distanceFromMouse = mouseWorldPos - m_playerTrans.position;

        // Find the position of the crosshair.
        Vector3 currentPos = transform.position - m_playerTrans.position;

        // Check if crosshair still needs to move to reach desired location.
        if (currentPos != distanceFromMouse)
        {
            Vector3 direction = mouseWorldPos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180; // Arbritrary 180 fixes incorrect rotation.

            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    // Attack Button Pressed.
    void Attack(InputAction.CallbackContext context)
    {
        if (!m_hasFired)
        {
            FireKnife();
        }
        else
        {
            m_isReturning = true;
        }
    }

    // On click spawn knife and fire in mouse direction.
    void FireKnife()
    {
        m_hasFired = true;

        // Get direction bullet needs to travel.
        Vector3 bulletDir = (m_firePointTrans.position - m_playerTrans.position).normalized;

        // Find rotation for spawned bullet.
        float radvalue = Mathf.Atan2(bulletDir.y, bulletDir.x);
        float angle = radvalue * Mathf.Rad2Deg;

        // Fire projectile in direction. Rotated to face direction.
        m_knifeReference = Instantiate(m_knifeObject, m_firePointTrans.position, Quaternion.identity);
        m_knifeReference.transform.Rotate(Vector3.forward, angle - 90); // 90 is an arbitrary value to fix offset.
        m_knifeReference.GetComponent<Rigidbody2D>().AddForce(bulletDir * 10, ForceMode2D.Impulse);
    }

    // Move towards player. Delete at destination.
    void ReturnKnife()
    {
        // Knife hit destination

        // DEFINITLY SHORTEN THIS.
        if (Mathf.Abs(m_knifeReference.transform.position.magnitude - m_playerTrans.position.magnitude) <= m_returnMagnitude)
        {
            Destroy(m_knifeReference.gameObject);
            m_knifeReference = null;

            m_isReturning = false;
            m_hasFired = false;
        }

        m_knifeReference.transform.position = Vector3.MoveTowards(m_knifeReference.transform.position, m_playerTrans.position, m_returnSpeed * Time.deltaTime);
    }
}
