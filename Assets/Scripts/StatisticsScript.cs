using TMPro;
using UnityEngine;

public class StatisticsScript : MonoBehaviour
{
    public static float m_damage;
    public static float m_speed;
    public static int m_coinCount { get; private set; }

    [SerializeField] TextMeshProUGUI m_coinText;

    public void IncrementCoinCounter()
    {
        m_coinCount++;

        Debug.Log("COLLECTION AW YEAH!");
    }

    public static void BuyUpgrade(int price, int damage, int speed)
    {
        if (m_coinCount >= price)
        {
            m_coinCount -= price;

            m_damage += damage;

            m_speed += speed;

            Debug.Log(m_coinCount);
            Debug.Log(m_damage);
            Debug.Log(m_speed);
        }
    }
}
