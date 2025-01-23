using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public float m_musicVolume { get; private set; }
    public float m_sfxVolume { get; private set; }

    // Audio Player.
    public AudioSource m_musicSource;

    // Slider references
    [SerializeField] Slider m_musicSlider;
    [SerializeField] Slider m_sfxSlider;

    void Start()
    {
        // Create Listeners.
        m_musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
        m_sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);

        // Set initial volume values.
        m_musicVolume = m_musicSlider.value;
        m_sfxVolume = m_sfxSlider.value;

        // Find the music player and set the volume again
        GameObject musicPlayer = GameObject.FindWithTag("Music Player");
        m_musicSource = musicPlayer.GetComponent<AudioSource>();
        m_musicSource.volume = m_musicVolume;

        // Subscribe to the activeSceneChanged event
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    void ChangedActiveScene(Scene current, Scene next)
    {
        Debug.Log("Hi");

        // Find the music player and set the volume again
        GameObject musicPlayer = GameObject.FindWithTag("Music Player");
        m_musicSource = musicPlayer.GetComponent<AudioSource>();
        m_musicSource.volume = m_musicVolume;
    }

    void OnMusicSliderValueChanged(float value)
    {
        m_musicVolume = value;
        m_musicSource.volume = m_musicVolume;
    }

    void OnSFXSliderValueChanged(float value)
    {
        Debug.Log("SFX Slider Value Changed: " + value);
        // Add your custom logic here for the SFX slider
    }
}
