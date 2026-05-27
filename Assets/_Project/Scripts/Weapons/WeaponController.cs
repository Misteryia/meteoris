using System;
using UnityEngine;

/// <summary>
/// Оркестрирует стрельбу: проверяет cooldown, спавнит снаряды из пула,
/// применяет recoil к турели. Тепловая система подключается в Фазе 6.
/// </summary>
public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private HeatSystem heatSystem;
    [SerializeField] private TurretAimer aimer;
    [SerializeField] private RailTurretController rail;
    [SerializeField] private ProjectilePool pool;

    public FireMode CurrentMode { get; private set; } = FireMode.Single;
    public event Action<FireMode> OnFireModeChanged;
    public event Action OnShotFired;

    private PlayerInput playerInput;
    private float cooldownTimer;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        if (heatSystem != null)
            heatSystem.Initialize(weaponData);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (playerInput.SwitchFireModePressed)
            SwitchFireMode();

        bool wantsToFire = CurrentMode == FireMode.Single
            ? playerInput.FirePressedThisFrame
            : playerInput.FireHeld;

        if (wantsToFire)
            TryFire();
    }

    public void SwitchFireMode()
    {
        CurrentMode = CurrentMode == FireMode.Single ? FireMode.Auto : FireMode.Single;
        OnFireModeChanged?.Invoke(CurrentMode);
    }

    public void TryFire()
    {
        if (cooldownTimer > 0f)
            return;

        float heatPerShot = CurrentMode == FireMode.Single
            ? weaponData.heatPerShotSingle
            : weaponData.heatPerShotAuto;

        if (heatSystem != null && !heatSystem.TryAddHeat(heatPerShot))
            return;

        float fireRate = CurrentMode == FireMode.Single
            ? weaponData.fireRateSingle
            : weaponData.fireRateAuto;

        float damage = CurrentMode == FireMode.Single
            ? weaponData.damageSingle
            : weaponData.damageAuto;

        float recoilImpulse = CurrentMode == FireMode.Single
            ? weaponData.recoilImpulseSingle
            : weaponData.recoilImpulseAuto;

        cooldownTimer = 1f / fireRate;

        // Спавн снаряда из пула
        Projectile projectile = pool.Get();
        projectile.transform.position = aimer.MuzzleWorldPosition;
        projectile.Damage = damage;
        projectile.Launch(aimer.AimDirection, weaponData.projectileSpeed, weaponData.projectileLifetime);

        // Отдача
        rail.ApplyRecoilImpulse(aimer.AimDirection, recoilImpulse);

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayShoot();

        OnShotFired?.Invoke();
    }
}
