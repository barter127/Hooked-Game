using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    /// <summary>
    /// Holds all static methods for visual FX.
    /// </summary>

    static GameObject m_bloodFX = GameObject.Find("Assets/Prefabs/FX/Blood FX.prefab");
    [SerializeField] AnimationCurve m_cameraShakeCurve;

    static VFXManager instance;

    void Start()
    {
        instance = this;
    }

    // Instantiate blood particle system at position.
    public static void SpawnBloodFX(Vector3 spawnPosition)
    {
        if (m_bloodFX != null)
        {
            Instantiate(m_bloodFX, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Blood FX prefab was lost");
        }
    }

    // Method to call camshake coroutine.
    public static void ShakeCamera(float duration)
    {
        instance.StartCoroutine(instance.ShakeCameraRoutine(duration));
    }

    // Coroutine to shake camera in random direction for duration seconds.
    private IEnumerator ShakeCameraRoutine(float duration)
    {
        Vector2 startPos = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Create multiplier based on animation curve.
            float shakeMultiplier = m_cameraShakeCurve.Evaluate(elapsedTime / duration);

            // Shake X and Y axis without affecting the z axis.
            Vector2 shakePos = startPos + Random.insideUnitCircle * shakeMultiplier;
            transform.position = new Vector3(shakePos.x, shakePos.y, transform.position.z);

            yield return null;
        }

        // Reset position after shaking.
        transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);
    }

    // Method to start coroutine for damage flash
    public static void FlashRed(SpriteRenderer spr, float duration)
    {
        // Stop current coroutine
        instance.StopCoroutine(instance.FlashRedRoutine(spr, duration));

        // Start/Renew the new one.
        instance.StartCoroutine(instance.FlashRedRoutine(spr, duration));
    }

    // Currently only works for sprites with colour white. Had issue if multiple coroutines ran at a time.
    private IEnumerator FlashRedRoutine(SpriteRenderer spr, float duration)
    {
        if (spr != null)
        {
            spr.color = Color.red;
        }

            yield return new WaitForSeconds(duration);

        if (spr != null)
        {
            spr.color = Color.white;
        }
    }
}
