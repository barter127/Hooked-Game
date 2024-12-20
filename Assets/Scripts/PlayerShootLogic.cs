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

    [SerializeField] LineRenderer m_lineRenderer;

    [Header ("Knife Return")]

    // Speed knife returns to player.
    [SerializeField] float m_returnSpeed;
    // Distance needed to destroy knife.
    [SerializeField] float m_returnMagnitude;

    bool m_hasFired;
    bool m_isReturning;
    GameObject m_knifeReference;
    Rigidbody2D m_knifeRb;

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

    private void Start()
    {
        m_lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateToMouse();


        if (m_hasFired)
        {
            RenderRope();
        }

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
            m_lineRenderer.enabled = true;
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
        m_knifeRb = m_knifeReference.GetComponent<Rigidbody2D>();
        m_knifeRb.AddForce(bulletDir * 10, ForceMode2D.Impulse);
    }

    void RenderRope()
    {
        m_lineRenderer.SetPosition(0, m_firePointTrans.position);
        m_lineRenderer.SetPosition(1, m_knifeReference.transform.position);
    }

    // Move towards player. Delete at destination.
    void ReturnKnife()
    {
        // Get direction bullet needs to travel.
        Vector3 bulletDir = (m_playerTrans.position - m_firePointTrans.position).normalized;

        float angle = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg + 90; // Arbritrary 180, fixes incorrect rotation.

        m_knifeReference.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        // Knife hit destination

        // DEFINITLY SHORTEN THIS.
        if ((Vector3.Distance(m_knifeReference.transform.position, m_playerTrans.position) <= m_returnMagnitude))
        {
            Destroy(m_knifeReference.gameObject);
            m_knifeReference = null;
            m_lineRenderer.enabled = false;

            m_isReturning = false;
            m_hasFired = false;
        }
        else
        {
            m_knifeReference.transform.position = Vector3.MoveTowards(m_knifeReference.transform.position, m_playerTrans.position, m_returnSpeed * Time.deltaTime);
        }
    }
}
