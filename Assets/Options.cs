using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    // Audio Player.
    AudioSource m_musicSource;

    // Slider references
    [SerializeField] Slider m_musicSlider;
    [SerializeField] Slider m_sfxSlider;

    void Start()
    {
        // Create Listeners.
        m_musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        m_sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);

        //
        GameObject musicPlayer = GameObject.FindWithTag("Music Player");
        m_musicSource = musicPlayer.GetComponent<AudioSource>();

        // Manually trigger event.
        OnMusicSliderValueChanged(m_musicSlider.value);
    }

    void OnMusicSliderValueChanged(float value)
    {
        m_musicSource.volume = value;
    }

    void OnSFXSliderValueChanged(float value)
    {
        Debug.Log("I am not functional");
    }
}
