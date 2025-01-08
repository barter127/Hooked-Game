using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] int startScene;

    public void StartButtonClicked()
    {
        SceneManager.LoadScene(startScene);
    }

    public void OptionsButtonClicked()
    {

    }

    public void ControlsButtonClicked()
    {

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
