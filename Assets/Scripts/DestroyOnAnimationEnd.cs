using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    // To be called through animation events.
    void AnimationEnd()
    {
        Destroy(gameObject);
    }
}
