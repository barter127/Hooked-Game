using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Variables :D

    [Header("Spawn Vars")]
    [SerializeField] float m_spawnRate;
    float m_nextSpawnTime;
    bool m_canSpawn = true;

    // References to Enemy GOs.
    [Header("Enemy Prefabs")]
    [SerializeField] GameObject m_hornet;
    [SerializeField] GameObject m_wasp;
    [SerializeField] GameObject m_squirt;
    [SerializeField] GameObject m_dip;

    [Header("Enemy Spawn Limits")]
    // Spawn limit: how many enemies can be in scene at once.
    // Spawn count: how many enemies are currently in scene.

    [SerializeField] int m_hornetSpawnLimit;
    int m_hornetSpawnCount;

    [SerializeField] int m_waspSpawnLimit;
    int m_waspSpawnCount;

    [SerializeField] int m_squirtSpawnLimit;
    int m_squirtSpawnCount;

    [SerializeField] int m_dipSpawnLimit;
    int m_dipSpawnCount;

    // Areas enemies can spawn. Assumes spawn area is square.
    [Header("Spawn Bounds")]
    [SerializeField] Transform m_topLeft;           [SerializeField] Transform m_topRight;
    [SerializeField] Transform m_bottomLeft;        [SerializeField] Transform m_bottomRight;

    // Cache possible values. (Saves calculation time)
    float m_minX; float m_maxX;
    float m_minY; float m_maxY;

    #endregion

    void Start()
    {
        m_nextSpawnTime = m_spawnRate + Time.time;

        // Set bound variables based of transforms in scene.
        m_minX = m_topLeft.position.x;
        m_maxX = m_topRight.position.x;
        m_minY = m_bottomLeft.position.y;
        m_minY = m_topLeft.position.y;
    }

    void Update()
    {
        if (m_canSpawn && m_nextSpawnTime <= Time.time)
        {
            m_nextSpawnTime = m_spawnRate + Time.time;

            SpawnRandomEnemy();
        }
    }

    // Get a random vector within the bounds of the spawn location.
    Vector3 RandomiseSpawnLocation()
    {
        float randX = Random.Range(m_minX, m_maxX);
        float randY = Random.Range(m_minY, m_maxY);

        return new Vector3(randX, randY, 0);
    }

    // Also contains checks for spawn cap.
    #region Spawn Methods

    // Spawn Random Enemy at Random position in spawn bounds.
    void SpawnRandomEnemy()
    {
        int randomEnemy = Random.Range(0, 4);

        switch (randomEnemy)
        {
            case 0: SpawnHornet(); break;
            case 1: SpawnWasp(); break;
            case 2: SpawnSquirt(); break;
            case 3: SpawnDip(); break;
        }
    }

    void SpawnHornet()
    {
        if (m_hornetSpawnCount < m_hornetSpawnLimit)
        {
            m_hornetSpawnCount++;
            Instantiate(m_hornet, RandomiseSpawnLocation(), Quaternion.identity);
        }
    }

    public void DecrementHornetCount()
    {
        m_hornetSpawnCount--;
    }

    void SpawnWasp()
    {
        if (m_waspSpawnCount < m_waspSpawnLimit)
        {
            m_waspSpawnCount++;
            Instantiate(m_wasp, RandomiseSpawnLocation(), Quaternion.identity);
        }
    }

    public void DecrementWaspCount()
    {
        m_waspSpawnCount--;
    }

    void SpawnSquirt()
    {
        if (m_squirtSpawnCount < m_squirtSpawnLimit)
        {
            m_squirtSpawnCount++;
            Instantiate(m_squirt, RandomiseSpawnLocation(), Quaternion.identity);
        }
    }

    public void DecrementSquirtCount()
    {
        m_squirtSpawnCount--;
    }

    void SpawnDip()
    {
        if (m_dipSpawnCount < m_dipSpawnLimit)
        {
            m_dipSpawnCount++;
            Instantiate(m_dip, RandomiseSpawnLocation(), Quaternion.identity);
        }
    }

    public void DecrementDipCount()
    {
        m_dipSpawnCount--;
    }

    #endregion
}
