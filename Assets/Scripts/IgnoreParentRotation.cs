using UnityEngine;

public class IgnoreParentRotation : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }
}
