using UnityEngine;
using static StraightToPathfinding;

public class StateMachine : MonoBehaviour
{
    // State Machine.
    public enum AIState
    {
        Idle,
        Moving,
        Attached,
        AttachedWeak
    }

    void Start()
    {
        m_currentState = AIState.Idle;
    }

    public AIState m_currentState { get; private set; }
    public AIState m_lastState { get; private set; }

    public void ChangeState(AIState newState)
    {
        // Prevent reverting back to attach state.
        if (newState != AIState.Attached && newState != AIState.AttachedWeak)
        {
            m_lastState = newState;
        }


        if (newState != m_currentState)
        {
            m_currentState = newState;
        }
    }
}
