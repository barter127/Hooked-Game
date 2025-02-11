using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    GameObject[] m_buyableTrans;

    void Start()
    {
        // Would be concerning for a large amount of items but fine for this scope.
        m_buyableTrans = GameObject.FindGameObjectsWithTag("Buyable");
    }

    IEnumerator RespawnItemAfterSeconds(float length)
    {
        GameObject

        yield return new WaitForSeconds(length);

    }
}
