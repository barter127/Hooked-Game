using UnityEngine;
using UnityEngine.Events;

public class CoinLogic : MonoBehaviour
{
    public UnityEvent m_coinCollected;

    [SerializeField] Rigidbody2D m_rigidbody;
    [SerializeField] Transform m_targetTransform;

    [SerializeField] StatisticsScript m_statsScript;

    [SerializeField] float m_speed;
   
    void Start()
    {
        if (m_targetTransform == null)
        {
            m_targetTransform = TransformReferenceHolder.m_player.transform;
        }


        StatisticsScript statisticsScript = GameObject.Find("Game Manager").GetComponent<StatisticsScript>();
        m_coinCollected.AddListener(statisticsScript.IncrementCoinCounter);
        m_coinCollected.AddListener(statisticsScript.UpdateStatisticsUI);

        //m_coinCollected.AddListener()

        ApplyInitialForce();
    }

    void FixedUpdate()
    {
        if (m_rigidbody.gravityScale <= 0f)
        {
            MoveTowardsPlayer();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_coinCollected.Invoke();

            Destroy(gameObject);
        }

    }

    void ApplyInitialForce()
    {
        // Randomise Upward Vector
        float randXDir = Random.Range(-1.0f, 1.0f);
        float randYDir = Random.Range(0f, 1.0f);

        // Create vector apply force.
        Vector2 fireDirection = new Vector2(randXDir, randYDir).normalized;
        m_rigidbody.AddForce(fireDirection, ForceMode2D.Impulse);
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (m_targetTransform.position - transform.position).normalized;
        m_rigidbody.AddForce(direction * m_speed);
    }
}
