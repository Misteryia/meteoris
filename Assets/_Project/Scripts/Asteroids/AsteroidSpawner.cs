using UnityEngine;

/// <summary>
/// Спавнит астероиды на сфере вокруг станции.
/// Выбирает размер по вероятностям из WaveConfig.
/// </summary>
public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Transform stationCenter;
    [SerializeField] private float spawnRadius = 50f;
    [SerializeField] private float angleDeviation = 15f;
    [SerializeField] private AsteroidData smallData;
    [SerializeField] private AsteroidData mediumData;
    [SerializeField] private AsteroidData largeData;
    [SerializeField] private Transform asteroidsContainer;

    /// <summary>Спавнит случайный астероид по параметрам волны.</summary>
    public Asteroid SpawnRandom(WaveConfig wave)
    {
        // Случайная точка на окружности спавна (плоскость XZ)
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spawnPos = stationCenter.position + new Vector3(
            Mathf.Cos(angle) * spawnRadius,
            0f,
            Mathf.Sin(angle) * spawnRadius
        );

        // Выбор размера по вероятностям
        AsteroidData data = PickSize(wave);

        GameObject go = Instantiate(data.prefab, spawnPos, Quaternion.identity, asteroidsContainer);

        Asteroid asteroid = go.GetComponent<Asteroid>();
        asteroid.Initialize(data, stationCenter);

        // Случайное отклонение от вектора на станцию (±15°)
        float deviation = Random.Range(-angleDeviation, angleDeviation);
        Vector3 toStation = (stationCenter.position - spawnPos).normalized;
        toStation = Quaternion.Euler(0f, deviation, 0f) * toStation;
        asteroid.SetDirection(toStation);

        return asteroid;
    }

    private AsteroidData PickSize(WaveConfig wave)
    {
        float total = wave.smallChance + wave.mediumChance + wave.largeChance;
        float roll = Random.Range(0f, total);

        if (roll < wave.smallChance)
            return smallData;
        if (roll < wave.smallChance + wave.mediumChance)
            return mediumData;
        return largeData;
    }
}
