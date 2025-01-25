using UnityEngine;

public class StartParticleSystem : MonoBehaviour
{
    // Turn particle system on play.
    void Start()
    {
        ParticleSystem particleSys = GetComponent<ParticleSystem>();
        particleSys.Play();
    }
}
