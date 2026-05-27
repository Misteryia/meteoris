using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Панель паузы. Открывается по Escape, замораживает время.
/// </summary>
public class PausePanel : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Slider volumeSlider;

    void Awake()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;

        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeClicked);
        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuClicked);
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        gameObject.SetActive(false);
    }

    private void HandleGameStateChanged(GameState state)
    {
        gameObject.SetActive(state == GameState.Paused);
    }

    private void OnResumeClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.TogglePause();
    }

    private void OnMenuClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadMainMenu();
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }
}
