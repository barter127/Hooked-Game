using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMusicManager : MonoBehaviour
{
    AudioSource m_audioData;

    void Start()
    {
        m_audioData.Play(0);
    }
}
