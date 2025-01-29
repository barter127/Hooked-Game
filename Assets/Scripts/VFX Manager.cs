using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    /// <summary>
    /// Holds all methods for visual FX.
    /// </summary>

    static GameObject m_bloodFX;
    [SerializeField] AnimationCurve m_cameraShakeCurve;

    
    VFXManager instance;

    void Start()
    {
        instance = this;

        // Will break if file is moved.
        m_bloodFX = GameObject.Find("Assets / Prefabs / FX / Blood FX.prefab");
        //m_camMovement = GameObject.Find("Camera").GetComponent<CameraMovement>();
    }

    //public static void ShakeCamera(float duration)
    //{
    //    instance.StartCoroutine(instance.ShakeCameraRoutine(duration));
    //}

    public static void SpawnBloodFX(Vector3 spawnPosition)
    {
        if (m_bloodFX ??= null)
        {
            Instantiate(m_bloodFX, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator ShakeCameraRoutine(float duration)
    {
        Vector2 startPos = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Create multiplier based on animation curve.
            float shakeMultiplier = m_cameraShakeCurve.Evaluate(elapsedTime / duration);

            // Shake X and Y axis without affecting z axis.
            Vector2 shakePos = startPos + Random.insideUnitCircle * shakeMultiplier;
            transform.position = new Vector3(shakePos.x, shakePos.y, transform.position.z);

            yield return null;
        }
    }
    // FIXXXXX!!!!

    //// Assumes sprites colour is already white
    //public static void FlashRed(SpriteRenderer spr, float duration)
    //{ 
    //    instance.StartCoroutine(instance.FlashRedRoutine(spr, duration));

    //}

    //private static IEnumerator FlashRedRoutine(SpriteRenderer spr, float duration)
    //{
    //    Color initialColour = spr.color;
    //    spr.color = Color.red;

    //    yield return new WaitForSeconds(duration);

    //    spr.color = initialColour;
    //}
}
