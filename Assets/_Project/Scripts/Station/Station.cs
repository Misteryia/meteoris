using System;
using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Космическая станция в центре арены.
/// Получает урон от астероидов при столкновении.
/// На GameObject: SphereCollider (isTrigger=true), layer=Station.
/// </summary>
public class Station : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDestroyed;

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0f) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0f);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (impulseSource != null)
            impulseSource.GenerateImpulse();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayStationHit();

        if (CurrentHealth <= 0f)
            OnDestroyed?.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        Asteroid asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            TakeDamage(asteroid.Data.damageToStation);
            asteroid.DestroyWithoutScore();
        }
    }
}
