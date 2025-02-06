using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class StraightToPathfinding : MonoBehaviour
{
    /// <summary>
    /// Move straight towards target object at consistent rate.
    /// Doesn't consider obstacles.
    /// Ability to pause movement for x time.
    /// </summary>


    // State Machine Reference.
    StateMachine m_stateMachine;

    // Movement and target
    [SerializeField] Transform m_targetTransform;
    [SerializeField] float m_speed;

    Rigidbody2D m_rigidbody;

    // Movement bools (restricts movement)
    bool m_canMove = true;

    // Direction to face.
    bool m_isFacingRight = false;
    SpriteRenderer m_spriteRenderer;

    // Idle jitter
    bool m_inIdleMovement = false;
    [SerializeField] float m_idleMoveRate;
    [SerializeField] float m_idleMoveDistance;
    Coroutine m_idleMoveIntervals;
    Vector2 m_idleMoveTarget;

    void Start()
    {
        m_stateMachine = GetComponent<StateMachine>();

        m_rigidbody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Target is valid.
        if (m_targetTransform != null)
        {
            // If in Idle Movement.
            if (m_inIdleMovement)
            {
                CheckDirectionToFace(transform.position.x > m_idleMoveTarget.x);
            }

            // Could be else, but without sprite to snap towards player after an idle movement.
            if (m_stateMachine.m_currentState != StateMachine.AIState.Idle)
            {
                // Flip sprite if nessecary.
                CheckDirectionToFace(transform.position.x > m_targetTransform.position.x);
            }
        }
    }

    void FixedUpdate()
    {
        if (m_stateMachine.m_currentState == StateMachine.AIState.Moving && m_canMove)
        {
            // Move at consistent rate towards target.
            Vector2 movePos = Vector2.MoveTowards(transform.position, m_targetTransform.position, m_speed * Time.fixedDeltaTime);
            m_rigidbody.MovePosition(movePos);
        }
        else if (m_stateMachine.m_currentState == StateMachine.AIState.Idle)
        {

            IdleMovementTimer();
            IdleMovement();
        }
    }

    #region Idle Movement

    void IdleMovement()
    {
        // Move to target.
        Vector2 movePos = Vector2.MoveTowards(transform.position, m_idleMoveTarget, m_speed * Time.deltaTime);
        m_rigidbody.MovePosition(movePos);

        // AI hit target position.
        if (Vector2.Distance(transform.position, m_idleMoveTarget) < 0.01f)
        {
            m_inIdleMovement = false;
        }
    }

    void IdleMovementTimer()
    {
        if (m_idleMoveIntervals == null)
        {
            m_idleMoveIntervals = StartCoroutine(IdleMoveIntervals());
        }
    }

    IEnumerator IdleMoveIntervals()
    {
        yield return new WaitForSeconds(m_idleMoveRate);

        // Create new target pos.
        m_idleMoveTarget = new Vector2 (transform.position.x + Random.insideUnitCircle.x * m_idleMoveDistance, 
                                        transform.position.y + Random.insideUnitCircle.y * m_idleMoveDistance);

        m_inIdleMovement = true;
        m_idleMoveIntervals = null;
    }

    #endregion

    #region Handle Vision Circle

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_stateMachine.m_currentState != StateMachine.AIState.Attached)
            {
                m_stateMachine.ChangeState(StateMachine.AIState.Moving);
                m_idleMoveTarget = transform.position; // Purposeful to stop movement.
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (m_stateMachine.m_currentState != StateMachine.AIState.Attached)
            {
                m_stateMachine.ChangeState (StateMachine.AIState.Idle);
                m_inIdleMovement = false;
                StopCoroutine(IdleMoveIntervals());
                m_idleMoveTarget = transform.position; // Purposeful to stop movement.
            }

        }
    }

    #endregion

    #region Pause Movement

    public void PauseAIMovement(float time)
    {
        StartCoroutine(PauseMovement(time));
    }

    IEnumerator PauseMovement(float time)
    {
        m_canMove = false;

        yield return new WaitForSeconds(time);

        m_canMove = true;
    }

    #endregion

    #region Direction to Face
    void Turn()
    {
        m_isFacingRight = !m_isFacingRight;
        m_spriteRenderer.flipX = m_isFacingRight;
    }

    void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != m_isFacingRight)
        {
            Turn();
        }
    }
    #endregion
}
