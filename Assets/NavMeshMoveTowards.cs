using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMoveTowards : MonoBehaviour
{
    [SerializeField] Transform m_targetTransform;
    NavMeshAgent m_agent;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        m_agent.SetDestination(m_targetTransform.position);

        if (m_agent.stoppingDistance > Vector2.Distance(transform.position, m_targetTransform.position))
        {
            if (!m_agent.isStopped)
            {
                StartCoroutine(PauseMovement());
            }
            Debug.Log("woah");
        }
    }

    IEnumerator PauseMovement()
    {
        m_agent.isStopped = true;

        yield return new WaitForSeconds(1.0f);

        m_agent.isStopped = false;
    }
}
