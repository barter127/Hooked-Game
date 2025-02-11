using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    /// <summary>
    /// Follow an object
    /// Would Need optimsation for more permanant GOs.
    /// </summary>

    public Transform m_transformToFollow;
    [SerializeField] Vector3 m_offsetFromTarget;

    void Start()
    {
        m_offsetFromTarget = transform.position - m_transformToFollow.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_transformToFollow != null)
        {
            transform.position = m_transformToFollow.position + m_offsetFromTarget;
        }
    }
}
