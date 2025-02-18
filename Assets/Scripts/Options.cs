using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
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

        // Subscribe to the activeSceneChanged event
        SceneManager.activeSceneChanged += ChangedActiveScene;

        SettingsData settingsData = JsonReadWriteSystem.LoadSettingsData();

        // File doesn't exist (probably because first time playing)
        if (settingsData == null)
        {
            SettingsData defaultSettings = new SettingsData();

            defaultSettings.musicVolume = 0.5f;
            defaultSettings.sfxVolume = 0.5f;

            JsonReadWriteSystem.SaveSettingsData(defaultSettings);

        }

        // Find the music player and set the volume again.
        GameObject musicPlayer = GameObject.FindWithTag("Music Player");
        m_musicSource = musicPlayer.GetComponent<AudioSource>();

        m_musicSource.volume = JsonReadWriteSystem.GetMusicVolume();

        // Make slider values match setting values.
        m_musicSlider.value = JsonReadWriteSystem.GetMusicVolume();
        m_sfxSlider.value = JsonReadWriteSystem.GetSFXVolume();

       
    }

    void ChangedActiveScene(Scene current, Scene next)
    {
        FindAndSetMusicPlayerVolume();

        FindAndSetSFXPlayers();
    }

    void FindAndSetMusicPlayerVolume()
    {
        // Find the music player and set the volume again.
        GameObject musicPlayer = GameObject.FindWithTag("Music Player");
        m_musicSource = musicPlayer.GetComponent<AudioSource>();
        m_musicSource.volume = JsonReadWriteSystem.GetMusicVolume();
    }

    void FindAndSetSFXPlayers()
    {
        AudioSource[] m_sfxPlayers = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource audioSource in m_sfxPlayers)
        {
            audioSource.volume = JsonReadWriteSystem.GetSFXVolume();
        }

    }

    void OnMusicSliderValueChanged(float value)
    {
        m_musicSource.volume = value;

        JsonReadWriteSystem.SetMusicVolume(value);
    }

    void OnSFXSliderValueChanged(float value)
    {
        JsonReadWriteSystem.SetSFXVolume(value);
    }
}
