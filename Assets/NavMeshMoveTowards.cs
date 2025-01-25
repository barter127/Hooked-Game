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

    private void Update()
    {
        m_agent.SetDestination(m_targetTransform.position);
    }
}
