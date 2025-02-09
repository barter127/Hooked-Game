using UnityEngine;

public class StatisticsScript : MonoBehaviour
{
    public float damage;
    public static float m_coinCount;

    public void IncrementCoinCounter()
    {
        m_coinCount++;

        Debug.Log("COLLECTION AW YEAH!");
    }
}
