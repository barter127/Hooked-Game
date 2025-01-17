using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Player location reference.
    [SerializeField] Transform m_playerTrans;
    // Controls shake intensity.
    [SerializeField] AnimationCurve m_animationCurve;

    void Update()
    {
        if (m_playerTrans != null)
        {
            Vector3 targetPos = new Vector3(m_playerTrans.position.x, m_playerTrans.position.y, transform.position.z);
            transform.position = targetPos;
        }
    }

    public void StartShake(float duration)
    { 
        StartCoroutine(ShakeCamera(duration));
    }

    IEnumerator ShakeCamera(float duration)
    {
        Vector2 startPos = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Create multiplier based on animation curve.
            float shakeMultiplier = m_animationCurve.Evaluate(elapsedTime/duration);

            // Shake X and Y axis without affecting z axis.
            Vector2 shakePos = startPos + Random.insideUnitCircle * shakeMultiplier;
            transform.position =  new Vector3(shakePos.x, shakePos.y, transform.position.z);
            
            yield return null;
        }
    }
}
