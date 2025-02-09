using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Rendering;

public class EnemyFireAtTarget : MonoBehaviour
{
    StateMachine m_stateMachine;

    [SerializeField] Transform m_target;

    [Header ("Firing Vars")]
    [SerializeField] GameObject bullet;

    [SerializeField] float m_fireTimerLength;
    float m_fireTimer;

    [SerializeField] float m_angleBetweenBullets = 5;

    [SerializeField] float m_pauseLength = 0.15f;

    [SerializeField] Transform m_firePoint;
    StraightToPathfinding m_movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_stateMachine = GetComponent<StateMachine>();
        m_movement = GetComponent<StraightToPathfinding>(); // Assumes GO has a movement component.

        // If no default target set to player.
        if (m_target == null)
        {
            m_target = TransformReferenceHolder.m_player.transform;
        }

        m_fireTimer = m_fireTimerLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_fireTimer <= 0)
        {
            if (m_stateMachine.m_currentState == StateMachine.AIState.Moving)
            {
                m_movement.PauseAIMovement(m_pauseLength);

                FireSinglebullet();
                m_fireTimer = m_fireTimerLength;
            }
            else if (m_stateMachine.m_currentState == StateMachine.AIState.Attached)
            {
                Attack();

                // Fires twice as fast.
                m_fireTimer = m_fireTimerLength / 2;
            }
        }
        // Only tick down in vision range.
        else m_fireTimer -= Time.deltaTime;
    }

    void Attack()
    {
        // Randomise Firing Type.
        bool doubleShot = Random.Range(0, 2) == 1;

        if (doubleShot)
        {
            FireDoublebullet();
        }

        else
        {
            FireSinglebullet();
        }
    }

    void FireSinglebullet()
    {
        // Get shooting Direction
        Vector3 bulletDir = (m_target.position - transform.position).normalized;

        // Spawn bullet and set its direction.
        GameObject bulletReference = Instantiate(bullet, m_firePoint.position, Quaternion.identity);
        bulletReference.GetComponent<BulletMovement>().SetDirection(bulletDir);
    }

    void FireDoublebullet()
    {
        // Get shooting direction.
        Vector3 aimingDir = (m_target.position - transform.position).normalized;

        // Find angle of aiming directions vector.
        float attackAngle = Mathf.Atan2(aimingDir.y, aimingDir.x);

        // This could be more customiseable but focusing on other AI.
        for (int i = -1; i < 2; i += 2)
        {
            // Get Direction to shoot each bullet (first iter negative, 2nd positive)
            Vector3 bulletDir = new Vector3(Mathf.Cos(attackAngle + Mathf.Deg2Rad * m_angleBetweenBullets * i), Mathf.Sin(attackAngle + Mathf.Deg2Rad * m_angleBetweenBullets * i), 0);

            // Spawn bullet and set its direction.
            GameObject bulletReference = Instantiate(bullet, m_firePoint.position, Quaternion.identity);
            bulletReference.GetComponent<BulletMovement>().SetDirection(bulletDir);
        }
    }

}
