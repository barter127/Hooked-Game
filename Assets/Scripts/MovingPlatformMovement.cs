using UnityEngine;

public class MovingPlatformMovement : MonoBehaviour
{
    [Header("Start and End")]
    // Vector3 so matches transform.
    Vector3 m_startPos;
    Vector3 m_endPos;
    [SerializeField] Transform m_endPosTransform;

    [Header("Rate of Movement")]
    [SerializeField] AnimationCurve m_movementCurve;
    [SerializeField] float m_timeToMove = 1;
    [SerializeField] float m_lerpSpeed = 1;
    float m_elapsedTime;
    bool m_movingToEnd;


    void Start()
    {
        // Get vector values.
        m_startPos = transform.position;
        m_endPos = m_endPosTransform.position;
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        // Get percentage complete.
        float percentComplete = m_movementCurve.Evaluate(m_elapsedTime / m_timeToMove);

        // Reset timer.
        if (percentComplete >= 1)
        {
            m_elapsedTime = 0;
            m_movingToEnd = !m_movingToEnd;

            return;
        }


        // Lerp position to target.
        if (m_movingToEnd) // Start to End.
        {
            transform.position = Vector3.Lerp(m_startPos, m_endPos, percentComplete);
        }
        else // End to Start.
        {
            transform.position = Vector3.Lerp(m_endPos, m_startPos, percentComplete);
        }

        m_elapsedTime += Time.deltaTime * m_lerpSpeed;
    }
}
