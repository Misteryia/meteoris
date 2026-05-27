using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Синглтон, управляющий состоянием игры и счётом.
/// Живёт только в сцене Game, DontDestroyOnLoad не нужен.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public int Score { get; private set; }

    [SerializeField] private Station station;
    [SerializeField] private WaveManager waveManager;

    public static event Action<GameState> OnGameStateChanged;
    public static event Action<int> OnScoreChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        if (station != null)
            station.OnDestroyed += HandleStationDestroyed;
        if (waveManager != null)
            waveManager.OnAllWavesCompleted += HandleVictory;
    }

    void OnDisable()
    {
        if (station != null)
            station.OnDestroyed -= HandleStationDestroyed;
        if (waveManager != null)
            waveManager.OnAllWavesCompleted -= HandleVictory;
    }

    private void HandleStationDestroyed()
    {
        EndGame(false);
    }

    private void HandleVictory()
    {
        EndGame(true);
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    public void StartGame()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
        SetState(GameState.Playing);

        if (waveManager != null)
            waveManager.StartWaves();
    }

    public void EndGame(bool victory)
    {
        SetState(victory ? GameState.Victory : GameState.GameOver);
    }

    public void AddScore(int amount)
    {
        if (CurrentState != GameState.Playing) return;
        Score += amount;
        OnScoreChanged?.Invoke(Score);
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            SetState(GameState.Paused);
        }
        else if (CurrentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            SetState(GameState.Playing);
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(CurrentState);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
