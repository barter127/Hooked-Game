using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class StraightToPathfinding : MonoBehaviour
{
    /// <summary>
    /// Move straight towards target object at consistent rate.
    /// Doesn't consider obstacles.
    /// Ability to pause movement for x time.
    /// </summary>

    [SerializeField] Transform m_targetTransform;
    [SerializeField] float m_speed;

    bool m_inView = false;
    bool m_canMove = true;

    bool m_isFacingRight = false;
    SpriteRenderer m_spriteRenderer;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (m_inView && m_canMove)
        {
            // Move at consistent rate towards target.
            transform.position = Vector2.MoveTowards(transform.position, m_targetTransform.position, m_speed * Time.fixedDeltaTime);
        }
    }

    #region Handle Vision Circle

    void OnTriggerEnter2D(Collider2D collision)
    {
        m_inView = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        m_inView = false;
    }

    #endregion

    #region Pause Movement

    public void PauseAIMovement(float time)
    {
        StartCoroutine(PauseMovement(time));
    }

    IEnumerator PauseMovement(float time)
    {
        Debug.Log(time);
        m_canMove = false;

        yield return new WaitForSeconds(time);

        m_canMove = true;
    }

    #endregion

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
