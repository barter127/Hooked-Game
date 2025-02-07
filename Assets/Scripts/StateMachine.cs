using UnityEngine;
using static StraightToPathfinding;

public class StateMachine : MonoBehaviour
{
    // State Machine.
    public enum AIState
    {
        Idle,
        Moving,
        Attached
    }

    private void Update()
    {
        //Debug.Log(m_currentState);
    }

    public AIState m_currentState { get; private set; }
    public AIState m_lastState { get; private set; }

    public void ChangeState(AIState newState)
    {
        m_lastState = m_currentState;
        m_currentState = newState;
    }
}
