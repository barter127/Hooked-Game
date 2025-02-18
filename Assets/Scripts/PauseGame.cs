using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PauseGame : MonoBehaviour
{
    /// <summary>
    /// Freezes game and brings up pause menu.
    /// Holds methods for Pause Menu Buttons.
    /// </summary>

    InputAction m_pauseAction;
    bool m_isPaused = false; // Used to check if game is paused.

    [SerializeField] GameObject m_pauseMenu;

    [SerializeField] int m_mainMenu;

    [SerializeField] StatisticsScript m_playerStats;
    [SerializeField] EnemySpawner m_enemySpawner;
    [SerializeField] HealthManager m_playerHealthManager;

    #region Handle Input Sys

    void OnEnable()
    {
        m_pauseAction = InputSystem.actions.FindAction("Pause");
        m_pauseAction.performed += PauseButton;
        m_pauseAction.Enable();

    }

    void OnDisable()
    {
        m_pauseAction.performed -= PauseButton;
        m_pauseAction.Disable();
    }

    #endregion

    void PauseButton(InputAction.CallbackContext context)
    {
        Pause();
    }

    // Pause and display UI if not paused. Unpause and remove UI if paused.
    void Pause()
    {
        // Check if paused or if game has been frozen for any other reason (death).
        if (!m_isPaused && Time.timeScale != 0)
        {
            m_isPaused = true;

            Time.timeScale = 0f;

            m_pauseMenu.SetActive(true);
        }
        else
        {
            m_isPaused = false;

            Time.timeScale = 1.0f;

            m_pauseMenu.SetActive(false);
        }
    }

    public void ResumeButtonClicked()
    {
        Pause();
    }

    public void RestartButtonClicked()
    {
        // Reset statistics
        m_playerStats.ResetStatistics();
        m_enemySpawner.ResetAllSpawnCounts();

        // Reload current scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }

    public void ExitButtonClicked()
    {
        // Load Main Menu Scene.
        SceneManager.LoadScene(m_mainMenu);
        Time.timeScale = 1.0f;
    }
}
