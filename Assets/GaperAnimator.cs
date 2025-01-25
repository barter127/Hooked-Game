using UnityEngine;

public class GaperAnimator : MonoBehaviour
{
    Animator m_animator;
    NavMeshMoveTowards m_movement;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_movement = GetComponent<NavMeshMoveTowards>();
    }

    private void Update()
    {
            m_animator.SetBool("Chasing", m_movement.m_isChasing);
    }
}
