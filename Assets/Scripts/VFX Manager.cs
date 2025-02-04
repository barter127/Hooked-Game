using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    /// <summary>
    /// Holds all static methods for visual FX.
    /// </summary>

    static GameObject m_bloodFX;
    [SerializeField] AnimationCurve m_cameraShakeCurve;

    static VFXManager instance;

    void Start()
    {
        instance = this;

        // Load the Blood FX prefab from the Resources folder.
        m_bloodFX = Resources.Load<GameObject>("Prefabs/FX/Blood FX");
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

    // Shakes camera in random direction
    IEnumerator ShakeCameraRoutine(float duration)
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
        instance.StartCoroutine(instance.FlashRedRoutine(spr, duration));
    }

    // Coroutine to flash red.
    private IEnumerator FlashRedRoutine(SpriteRenderer spr, float duration)
    {
        Color initialColour = spr.color;
        spr.color = Color.red;

        yield return new WaitForSeconds(duration);

        spr.color = initialColour;
    }
}
