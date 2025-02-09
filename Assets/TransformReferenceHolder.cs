using UnityEngine;

public class TransformReferenceHolder : MonoBehaviour
{
    public static Transform m_player;

    void Awake()
    {
        GameObject.Find("Character");
        m_player = GetComponent<Transform>();
    }
}
