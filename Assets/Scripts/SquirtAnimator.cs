using UnityEngine;

[RequireComponent(typeof(ChargeTowardsPoint))]
[RequireComponent(typeof(Animator))]
public class SquirtAnimator : MonoBehaviour
{
    ChargeTowardsPoint m_movement;
    Animator m_Animator;

    void Start()
    {
        m_movement = GetComponent<ChargeTowardsPoint>();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_movement.m_startCharge)
        {
            m_Animator.SetTrigger("Start Charge");

            m_movement.m_startCharge = false;
        }
    }
}
