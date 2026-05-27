using System;
using UnityEngine;

/// <summary>
/// Астероид. Летит к станции по фиксированному направлению.
/// При HP ≤ 0 уничтожается и оповещает подписчиков.
/// На префабе: Rigidbody (kinematic) + SphereCollider (НЕ trigger).
/// </summary>
public class Asteroid : MonoBehaviour
{
    public event Action<Asteroid> OnDestroyed;
    public AsteroidData Data { get; private set; }
    public float CurrentHealth { get; private set; }

    private Vector3 moveDirection;
    private Vector3 stationPosition;
    private Rigidbody rb;
    private float maxDistanceSqr;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>Инициализирует астероид данными и целью (станцией).</summary>
    public void Initialize(AsteroidData data, Transform target)
    {
        Data = data;
        CurrentHealth = data.maxHealth;
        stationPosition = target.position;

        // Если астероид улетел дальше радиуса спавна — уничтожить
        float spawnDist = (transform.position - stationPosition).magnitude;
        maxDistanceSqr = (spawnDist + 10f) * (spawnDist + 10f);

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;
        moveDirection = toTarget.normalized;
    }

    /// <summary>Задаёт направление движения напрямую (для отклонения при спавне).</summary>
    public void SetDirection(Vector3 direction)
    {
        direction.y = 0f;
        moveDirection = direction.normalized;
    }

    void FixedUpdate()
    {
        if (Data == null) return;
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing) return;
        rb.MovePosition(rb.position + moveDirection * Data.speed * Time.fixedDeltaTime);

        // Астероид пролетел мимо — уничтожить без очков и без VFX
        float distSqr = (rb.position - stationPosition).sqrMagnitude;
        if (distSqr > maxDistanceSqr)
        {
            OnDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0f)
            Die(true);
    }

    /// <summary>Уничтожает астероид без начисления очков (столкновение со станцией).</summary>
    public void DestroyWithoutScore()
    {
        Die(false);
    }

    private void Die(bool giveScore)
    {
        if (giveScore && Data != null && GameManager.Instance != null)
            GameManager.Instance.AddScore(Data.scoreReward);

        if (Data != null && Data.explosionVFXPrefab != null)
            VFXSpawner.Spawn(Data.explosionVFXPrefab, transform.position);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayExplosion(transform.position);

        OnDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
