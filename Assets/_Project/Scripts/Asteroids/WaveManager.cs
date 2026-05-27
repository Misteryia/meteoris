using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Оркестратор волн. Последовательно запускает волны из списка WaveConfig,
/// спавнит астероиды через AsteroidSpawner, отслеживает прогресс.
/// </summary>
public class WaveManager : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner spawner;
    [SerializeField] private List<WaveConfig> waves;

    public int CurrentWaveIndex { get; private set; }
    public int AsteroidsSpawned { get; private set; }
    public int AsteroidsRemaining { get; private set; }

    public event Action<int> OnWaveStarted;
    public event Action<int, int> OnWaveProgressChanged;
    public event Action OnAllWavesCompleted;

    private int activeAsteroidCount;

    public void StartWaves()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            CurrentWaveIndex = i;
            WaveConfig wave = waves[i];

            AsteroidsSpawned = 0;
            activeAsteroidCount = 0;
            OnWaveStarted?.Invoke(i + 1);

            Coroutine spawnRoutine = StartCoroutine(SpawnRoutine(wave));

            if (wave.isFinalWave)
            {
                // Финальная волна — продержаться 60 секунд
                yield return new WaitForSeconds(60f);
                OnAllWavesCompleted?.Invoke();
                yield break;
            }

            // Ждём пока пройдёт время волны ИЛИ все астероиды уничтожены
            float timer = 0f;
            while (timer < wave.duration || activeAsteroidCount > 0)
            {
                timer += Time.deltaTime;

                // Если время вышло и все заспавненные астероиды убиты — переходим
                if (timer >= wave.duration && activeAsteroidCount <= 0)
                    break;

                yield return null;
            }
        }

        OnAllWavesCompleted?.Invoke();
    }

    private IEnumerator SpawnRoutine(WaveConfig wave)
    {
        int toSpawn = wave.isFinalWave ? int.MaxValue : wave.totalAsteroids;

        while (AsteroidsSpawned < toSpawn)
        {
            if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
                yield break;

            Asteroid asteroid = spawner.SpawnRandom(wave);
            asteroid.OnDestroyed += HandleAsteroidDestroyed;
            AsteroidsSpawned++;
            activeAsteroidCount++;

            AsteroidsRemaining = wave.isFinalWave ? activeAsteroidCount : (toSpawn - AsteroidsSpawned + activeAsteroidCount);
            OnWaveProgressChanged?.Invoke(AsteroidsSpawned, toSpawn);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void HandleAsteroidDestroyed(Asteroid asteroid)
    {
        asteroid.OnDestroyed -= HandleAsteroidDestroyed;
        activeAsteroidCount--;
    }
}
