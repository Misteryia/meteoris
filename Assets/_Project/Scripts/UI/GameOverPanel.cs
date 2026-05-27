using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Панель Game Over / Victory. Показывается при завершении игры.
/// Подписывается на событие в Awake (до деактивации), потому что панель
/// начинает деактивированной и OnEnable не вызовется.
/// </summary>
public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    void Awake()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
        if (menuButton != null)
            menuButton.onClick.AddListener(OnMenuClicked);
    }

    void OnDisable()
    {
        if (restartButton != null)
            restartButton.onClick.RemoveListener(OnRestartClicked);
        if (menuButton != null)
            menuButton.onClick.RemoveListener(OnMenuClicked);
    }

    void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.GameOver || state == GameState.Victory)
            Show(state);
    }

    private void Show(GameState state)
    {
        gameObject.SetActive(true);

        if (titleText != null)
            titleText.text = state == GameState.Victory ? "ПОБЕДА" : "ПОРАЖЕНИЕ";

        if (finalScoreText != null && GameManager.Instance != null)
            finalScoreText.text = $"Очки: {GameManager.Instance.Score}";
    }

    private void OnRestartClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.RestartScene();
    }

    private void OnMenuClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.LoadMainMenu();
    }
}
