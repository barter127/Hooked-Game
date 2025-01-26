using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;

    bool m_isFacingRight;

    void Update()
    {
        
    }

    #region Direction to Face
    void Turn()
    {
        m_isFacingRight = !m_isFacingRight;
        m_spriteRenderer.flipX = m_isFacingRight;
    }

    void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != m_isFacingRight)
        {
            Turn();
        }
    }
    #endregion
}
