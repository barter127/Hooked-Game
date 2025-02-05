using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    /// <summary>
    /// Contains methods for all buttons in main menu
    /// Centralised logic was preferred as most methods are very small and simple.
    /// </summary>

    [SerializeField] int m_startScene;

    [SerializeField] GameObject m_mainMenuPanel;
    [SerializeField] GameObject m_optionsPanel;
    [SerializeField] GameObject m_controlsPanel;
    [SerializeField] GameObject m_statPanel;

    public void StartButtonClicked()
    {
        SceneManager.LoadScene(m_startScene);
    }

    public void OptionsButtonClicked()
    {
        m_mainMenuPanel.SetActive(false);
        m_optionsPanel.SetActive(true);
    }

    public void ControlsButtonClicked()
    {
        m_mainMenuPanel.SetActive(false);
        m_controlsPanel.SetActive(true);
    }

    public void StatsButtonClicked()
    {
        m_mainMenuPanel.SetActive(false);
        m_statPanel.SetActive(true);
    }

    public void XButtonClicked()
    {
        // Suboptimal but allows method to be reused.
        // Performance loss is fractional don't bother optimising.
        m_optionsPanel.SetActive(false);
        m_controlsPanel.SetActive(false);
        m_statPanel.SetActive(false);

        m_mainMenuPanel.SetActive(true);
    }

    public void ExitButtonClicked()
    {
        #if UNITY_STANDALONE
                Application.Quit();
        #endif
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
