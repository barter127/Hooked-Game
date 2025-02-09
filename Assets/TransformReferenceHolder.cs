using UnityEngine;

public class TransformReferenceHolder : MonoBehaviour
{
    public static Transform m_player;

    void Awake()
    {
        GameObject player = GameObject.Find("Character");
        m_player = player.GetComponent<Transform>();
    }
}
