using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            blood.Play();
        }
    }
}
