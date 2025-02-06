using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    StateMachine m_stateMachine;

    [Header ("Firing Vars")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform m_target;

    [SerializeField] float m_fireTimerLength;
    [SerializeField] float m_pauseLength;
    float m_fireTimer;

    [SerializeField] Transform m_firePoint;
    StraightToPathfinding m_movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_stateMachine = GetComponent<StateMachine>();
        m_movement = GetComponent<StraightToPathfinding>();

        m_fireTimer = m_fireTimerLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_fireTimer <= 0 && m_stateMachine.m_currentState == StateMachine.AIState.Moving)
        {
            m_movement.PauseAIMovement(m_pauseLength);

            FireBullet();

            m_fireTimer = m_fireTimerLength;
        }
        else m_fireTimer -= Time.deltaTime;
    }

    void FireBullet()
    {
        Vector3 bulletDir = (m_target.position - transform.position).normalized;

        GameObject bulletReference = Instantiate(bullet, m_firePoint.position, Quaternion.identity);
        bulletReference.GetComponent<BulletMovement>().SetDirection(bulletDir);
    }
}
