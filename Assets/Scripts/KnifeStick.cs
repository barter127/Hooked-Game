using UnityEngine;

public class KnifeStick : MonoBehaviour
{
    Rigidbody2D m_rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            transform.SetParent(collision.transform, true);
            m_rigidbody.linearVelocity = Vector2.zero;
            m_rigidbody.bodyType = RigidbodyType2D.Kinematic; 
        }
        
        //m_rigidbody.bodyType = RigidbodyType2D.Static;
    }
}
