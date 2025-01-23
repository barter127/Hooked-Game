using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class to control the top down character.
/// Implements the player controls for moving.
/// Updates the player animator so the character animates based on input.
/// </summary>
public class TopDownCharacterController : MonoBehaviour
{
    #region Framework Variables

    //The inputs that we need to retrieve from the input system.
    InputAction m_moveAction;
    InputAction m_lookAction;

    //The components that we need to edit to make the player move smoothly.
    Animator m_animator;
    Rigidbody2D m_rigidbody;
    
    //The direction that the player is moving in.
    Vector2 m_playerDirection;
   

    [Header("Movement parameters")]
    // Rate the player accelerates at.
    [SerializeField] float m_playerAccelRate;
    // The maximum speed the player can move.
    [SerializeField] float m_playerMaxSpeed = 1000f;

    #endregion

    private void Start()
    {
        // Bind movement inputs to variables.
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_lookAction = InputSystem.actions.FindAction("Look");
        
        // Get components from Character game object so that we can use them later.
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get difference between targetSpeed and maxSpeed
        // Adjust velocity based off of this.
        float xTarget = m_playerDirection.x * m_playerMaxSpeed;
        float xSpeedDif = xTarget - m_rigidbody.linearVelocity.x;
        float xMovement = xSpeedDif * m_playerAccelRate;

        float yTarget = m_playerDirection.y * m_playerMaxSpeed;
        float ySpeedDif = yTarget - m_rigidbody.linearVelocity.y;
        float yMovement = ySpeedDif * m_playerAccelRate;

        // Apply rb force. 
        m_rigidbody.AddForce(xMovement * Vector2.right, ForceMode2D.Force);
        m_rigidbody.AddForce(yMovement * Vector2.up, ForceMode2D.Force);
    }
    
    void Update()
    {
        // Store any movement inputs into m_playerDirection - this will be used in FixedUpdate to move the player.
        m_playerDirection = m_moveAction.ReadValue<Vector2>();

        // ~~ handle animator ~~
        // Update the animator speed to ensure that we revert to idle if the player doesn't move.
        m_animator.SetFloat("Speed", m_playerDirection.magnitude);

        if (!PlayerShootLogic.M_HasFired)
        {
            // Get mouse position in world space
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(transform.position.z - transform.position.z); // Set z distance from the camera
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Calculate direction to mouse
            Vector3 direction = worldMousePosition - transform.position;
            direction.z = 0; // Ignore z-axis
            direction.Normalize();

            // Set Animator parameters
            m_animator.SetFloat("Horizontal", direction.x);
            m_animator.SetFloat("Vertical", direction.y);
        }
        else
        {
            m_animator.SetFloat("Horizontal", m_playerDirection.x);
            m_animator.SetFloat("Vertical", m_playerDirection.y);
        }
    }
}
