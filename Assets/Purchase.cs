using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Purchase : MonoBehaviour
{
    [SerializeField] TextMeshPro m_moneyText;

    [Header ("Stats")]
    [SerializeField] int m_price;
    [SerializeField] int m_damage;
    [SerializeField] int m_speed;

    public UnityEvent m_itemBought;

    void Start()
    {
        // Ensure Text value is correct.
        m_moneyText.text = "£" + m_price.ToString();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StatisticsScript.BuyUpgrade(m_price, m_damage, m_speed);
        }
    }
}
