using UnityEngine;

public class DestroyOnAnimationEnd : MonoBehaviour
{
    void AnimationEnd()
    {
        Destroy(gameObject);
    }
}
