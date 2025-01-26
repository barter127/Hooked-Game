using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(NavMeshAgent))]
public class NavMeshMoveTowards : MonoBehaviour
{
    [SerializeField] Transform m_targetTransform;
    NavMeshAgent m_agent;
    SpriteRenderer m_spriteRenderer;

    // Time spent paused after getting in stopping range.
    [SerializeField] float pauseLength;
    public bool m_isChasing = true; // For animator.

    bool m_isFacingRight = true;
    bool m_inView = false;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_isChasing = true;
    }

    void Update()
    {
        if (m_inView)
        {
            m_agent.SetDestination(m_targetTransform.position);

            // In stopping range.
            float distance = Vector2.Distance(transform.position, m_targetTransform.position);
            if (m_agent.stoppingDistance > distance)
            {
                PauseAIMovement(pauseLength);
            }
        }

        // Check direction to face.
        float relativeXPos = m_targetTransform.position.x - transform.position.x;
        CheckDirectionToFace(relativeXPos < 0);
    }

    #region Handle Vision Circle

    void OnTriggerEnter2D(Collider2D collision)
    {
        m_inView = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        m_inView = false;
    }

    #endregion

    #region Pause Movement

    public void PauseAIMovement(float time)
    {
        StartCoroutine(PauseMovement(time));
    }

    IEnumerator PauseMovement(float time)
    {
        Debug.Log(time);
        m_isChasing = false;
        m_agent.isStopped = true;

        yield return new WaitForSeconds(time);

        m_isChasing = true;
        m_agent.isStopped = false;
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
