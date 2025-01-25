using UnityEngine;

[RequireComponent (typeof (RandomisedMovement))]
[RequireComponent (typeof (Animator))]
public class DipAnimator : MonoBehaviour
{
    RandomisedMovement m_movement;
    Animator m_animator;

    private void Start()
    {
        m_movement = GetComponent<RandomisedMovement>();   
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        // When movement script charges turn on 
        if (m_movement.m_startCharge)
        {
            m_animator.SetTrigger("Start Charge");
            m_movement.m_startCharge = false;
        }
    }
}
