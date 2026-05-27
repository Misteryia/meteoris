using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Контроллер главного меню. Две кнопки: начать игру и выйти.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public void OnStartClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
