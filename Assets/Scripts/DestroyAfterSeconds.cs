using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] float m_lifeSpan;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, m_lifeSpan);
    }
}
