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

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_isChasing = true;
    }

    void Update()
    {
        m_agent.SetDestination(m_targetTransform.position);

        float relativeXPos = m_targetTransform.position.x - transform.position.x;
        CheckDirectionToFace(relativeXPos < 0);

        // In stopping range.
        float distance = Vector2.Distance(transform.position, m_targetTransform.position);
        if (m_agent.stoppingDistance > distance)
        {
            PauseAIMovement(pauseLength);
        }
    }

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
}
