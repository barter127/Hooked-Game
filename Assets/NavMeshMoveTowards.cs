using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMoveTowards : MonoBehaviour
{
    [SerializeField] Transform m_targetTransform;
    NavMeshAgent m_agent;

    // Time spent paused after getting in stopping range.
    [SerializeField] float pauseLength;
    public bool m_isChasing = true; // For animator.

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        m_isChasing = true;
    }

    void Update()
    {
        m_agent.SetDestination(m_targetTransform.position);

        // In stopping range.
        float distance = Vector2.Distance(transform.position, m_targetTransform.position);
        if (m_agent.stoppingDistance > distance)
        {
            Debug.Log("HI");
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
}
