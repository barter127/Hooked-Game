using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class GravityScaleDrag : MonoBehaviour
{
    /// <summary>
    ///  Reduce gravity scale during runtime.
    /// </summary>

    float m_gravityScale = 1;
    [SerializeField] Rigidbody2D m_rigidbody;

    void FixedUpdate()
    {
        if (m_gravityScale > 0)
        {
            m_gravityScale -= Time.deltaTime;

            m_rigidbody.gravityScale = m_gravityScale;
        }
    }
}
