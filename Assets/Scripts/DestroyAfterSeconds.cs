using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    float bulletTimer;
    // Lifetime of bullet in seconds.
    [SerializeField] float bulletTimerLength;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletTimer = bulletTimerLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (bulletTimer < 0) Destroy(gameObject);
        else bulletTimer -= Time.deltaTime;
    }
}
