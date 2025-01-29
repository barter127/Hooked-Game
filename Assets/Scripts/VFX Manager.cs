using System.Collections;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    /// <summary>
    /// Holds all methods for visual FX.
    /// </summary>

    static GameObject m_bloodFX;
    
    static CameraMovement m_camMovement;

    
    VFXManager instance;

    void Start()
    {
        instance = this;

        // Will break if file is moved.
        m_bloodFX = GameObject.Find("Assets / Prefabs / FX / Blood FX.prefab");
        m_camMovement = GameObject.Find("Camera").GetComponent<CameraMovement>();
    }

    public static void ShakeCamera(float duration)
    {
        m_camMovement.StartCoroutine(m_camMovement.ShakeCamera(duration));
    }

    public static void SpawnBloodFX(Vector3 spawnPosition)
    {
        if (m_bloodFX ??= null)
        {
            Instantiate(m_bloodFX, spawnPosition, Quaternion.identity);
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
