using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Тепловая система оружия. Нет патронов — есть тепло.
/// Перегрев блокирует стрельбу на overheatLockoutDuration секунд.
/// </summary>
public class HeatSystem : MonoBehaviour
{
    public float CurrentHeat { get; private set; }
    public float MaxHeat { get; private set; } = 100f;
    public bool IsOverheated { get; private set; }
    public float HeatNormalized => MaxHeat > 0f ? CurrentHeat / MaxHeat : 0f;

    public event Action<float, float> OnHeatChanged;
    public event Action OnOverheatStart;
    public event Action OnOverheatEnd;

    private float dissipationRate;
    private float overheatCooldownRate;
    private float overheatLockoutDuration;

    public void Initialize(WeaponData data)
    {
        MaxHeat = data.maxHeat;
        dissipationRate = data.heatDissipationRate;
        overheatCooldownRate = data.overheatCooldownRate;
        overheatLockoutDuration = data.overheatLockoutDuration;
        CurrentHeat = 0f;
    }

    void Update()
    {
        if (IsOverheated)
        {
            CurrentHeat -= overheatCooldownRate * Time.deltaTime;
        }
        else if (CurrentHeat > 0f)
        {
            CurrentHeat -= dissipationRate * Time.deltaTime;
        }

        CurrentHeat = Mathf.Max(CurrentHeat, 0f);
        OnHeatChanged?.Invoke(CurrentHeat, MaxHeat);
    }

    /// <summary>
    /// Пытается добавить тепло. Возвращает false если перегрев активен.
    /// </summary>
    public bool TryAddHeat(float amount)
    {
        if (IsOverheated) return false;

        CurrentHeat += amount;
        OnHeatChanged?.Invoke(CurrentHeat, MaxHeat);

        if (CurrentHeat >= MaxHeat)
        {
            CurrentHeat = MaxHeat;
            StartCoroutine(OverheatRoutine());
        }

        return true;
    }

    private IEnumerator OverheatRoutine()
    {
        IsOverheated = true;
        OnOverheatStart?.Invoke();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayOverheatStart();

        yield return new WaitForSeconds(overheatLockoutDuration);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayOverheatEnd();

        IsOverheated = false;
        OnOverheatEnd?.Invoke();
    }
}
