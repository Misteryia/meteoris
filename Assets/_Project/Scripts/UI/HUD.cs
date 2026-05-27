using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Слушает события всех игровых систем и обновляет элементы UI.
/// </summary>
public class HUD : MonoBehaviour
{
    [Header("Здоровье станции")]
    [SerializeField] private Slider stationHealthBar;

    [Header("Тепло")]
    [SerializeField] private Slider heatBar;
    [SerializeField] private Image heatBarFill;
    [SerializeField] private GameObject overheatWarning;

    [Header("Текст")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI fireModeText;

    [Header("Ссылки на системы")]
    [SerializeField] private Station station;
    [SerializeField] private HeatSystem heatSystem;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private WaveManager waveManager;

    private Color normalHeatColor = new Color(1f, 0.6f, 0f);
    private Color overheatColor = Color.red;

    void OnEnable()
    {
        if (station != null)
            station.OnHealthChanged += UpdateHealthBar;

        if (heatSystem != null)
        {
            heatSystem.OnHeatChanged += UpdateHeatBar;
            heatSystem.OnOverheatStart += ShowOverheatWarning;
            heatSystem.OnOverheatEnd += HideOverheatWarning;
        }

        if (weaponController != null)
            weaponController.OnFireModeChanged += UpdateFireModeText;

        if (waveManager != null)
            waveManager.OnWaveStarted += UpdateWaveText;

        GameManager.OnScoreChanged += UpdateScoreText;
    }

    void OnDisable()
    {
        if (station != null)
            station.OnHealthChanged -= UpdateHealthBar;

        if (heatSystem != null)
        {
            heatSystem.OnHeatChanged -= UpdateHeatBar;
            heatSystem.OnOverheatStart -= ShowOverheatWarning;
            heatSystem.OnOverheatEnd -= HideOverheatWarning;
        }

        if (weaponController != null)
            weaponController.OnFireModeChanged -= UpdateFireModeText;

        if (waveManager != null)
            waveManager.OnWaveStarted -= UpdateWaveText;

        GameManager.OnScoreChanged -= UpdateScoreText;
    }

    void Start()
    {
        if (overheatWarning != null)
            overheatWarning.SetActive(false);

        UpdateScoreText(0);
        UpdateFireModeText(FireMode.Single);
        UpdateWaveText(1);

        if (station != null && stationHealthBar != null)
        {
            stationHealthBar.maxValue = station.MaxHealth;
            stationHealthBar.value = station.MaxHealth;
        }

        if (heatBar != null)
        {
            heatBar.maxValue = 1f;
            heatBar.value = 0f;
        }
    }

    private void UpdateHealthBar(float current, float max)
    {
        if (stationHealthBar != null)
        {
            stationHealthBar.maxValue = max;
            stationHealthBar.value = current;
        }
    }

    private void UpdateHeatBar(float current, float max)
    {
        if (heatBar != null)
            heatBar.value = max > 0f ? current / max : 0f;
    }

    private void ShowOverheatWarning()
    {
        if (overheatWarning != null)
            overheatWarning.SetActive(true);

        if (heatBarFill != null)
            heatBarFill.color = overheatColor;
    }

    private void HideOverheatWarning()
    {
        if (overheatWarning != null)
            overheatWarning.SetActive(false);

        if (heatBarFill != null)
            heatBarFill.color = normalHeatColor;
    }

    private void UpdateScoreText(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Очки: {score}";
    }

    private void UpdateWaveText(int waveNumber)
    {
        if (waveText != null)
            waveText.text = $"Волна {waveNumber}";
    }

    private void UpdateFireModeText(FireMode mode)
    {
        if (fireModeText != null)
            fireModeText.text = mode == FireMode.Single ? "Одиночный" : "Автоогонь";
    }
}
