using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Purchase : MonoBehaviour
{
    [Header("References")]
    // Price Text on object.
    [SerializeField] TextMeshPro m_moneyText;

    // Certain children will need to be turned off such as renderer and movement logic.
    [SerializeField] GameObject m_goToTurnOff;

    // Can currently be bought.
    bool m_avalible = true;

    // Item can respawn
    [SerializeField] bool m_respawnOnPurchase;

    // Time for item to reappear after purchase.
    float m_respawnTimeLength = 1;

    [Header ("Stats")]
    [SerializeField] int m_price;
    [SerializeField] int m_damage;
    [SerializeField] int m_speed;

    public UnityEvent m_itemBought;

    void Start()
    {
        // Ensure money value is correct.
        m_moneyText.text = "£" + m_price.ToString();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && m_avalible)
        {
            // I prefer check here as it's linked to Particle FXs
            if (StatisticsScript.m_coinCount >= m_price)
            {
                StatisticsScript.BuyUpgrade(m_price, m_damage, m_speed);
                m_itemBought.Invoke();

                // Spawn at pos. Follow Player.
                VFXManager.SpawnSparkleFX(TransformReferenceHolder.m_player.position, TransformReferenceHolder.m_player);

                StartCoroutine(RespawnAfterSeconds(m_respawnTimeLength));
            }
        }
    }

    IEnumerator RespawnAfterSeconds(float length)
    {
        m_goToTurnOff.SetActive(false);
        m_avalible = false;

        yield return new WaitForSeconds(length);

        m_goToTurnOff.SetActive(true);
        m_avalible = true;
    }
}
