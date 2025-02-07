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

    private void Update()
    {
        //Debug.Log(m_currentState);
    }

    public AIState m_currentState { get; private set; }
    public AIState m_lastState { get; private set; }

    public void ChangeState(AIState newState)
    {
        if (newState != AIState.Attached && newState != AIState.AttachedWeak)
        {
            m_lastState = m_currentState;
        }

            m_currentState = newState;
    }
}
