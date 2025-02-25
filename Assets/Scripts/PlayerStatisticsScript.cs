using TMPro;
using UnityEngine;

public class StatisticsScript : MonoBehaviour
{
    /// <summary>
    /// Holds statistic values and multiplers for player.
    /// </summary>

    public static float m_damage { get; private set; } = 5;
    public static float m_speed { get; private set; }  = 12;
    public static int m_coinCount { get; private set; }

    [SerializeField] TextMeshProUGUI m_coinText;

    public void IncrementCoinCounter()
    {
        m_coinCount++;
    }

    public static void BuyUpgrade(int price, int damage, int speed)
    {
        m_coinCount -= price;

        m_damage += damage;

        m_speed += speed;

        Debug.Log(m_damage);
        Debug.Log(m_speed);
    }

    public void ResetStatistics()
    {
        m_coinCount = 0;
        m_damage = 5;
        m_speed = 12;

        UpdateStatisticsUI();
    }

    public void UpdateStatisticsUI()
    {
        m_coinText.text = m_coinCount.ToString();
    }
}
