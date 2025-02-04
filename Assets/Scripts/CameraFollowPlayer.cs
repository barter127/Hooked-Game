using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Player location reference.
    [SerializeField] Transform m_playerTrans;

    void Update()
    {
        if (m_playerTrans != null)
        {
            Vector3 targetPos = new Vector3(m_playerTrans.position.x, m_playerTrans.position.y, transform.position.z);
            transform.position = targetPos;
        }
    }
}
