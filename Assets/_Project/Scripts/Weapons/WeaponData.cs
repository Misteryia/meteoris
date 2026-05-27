using UnityEngine;

/// <summary>ScriptableObject со всеми параметрами оружия.</summary>
[CreateAssetMenu(menuName = "Game/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Урон и скорострельность")]
    public float damageSingle = 2f;
    public float damageAuto = 1f;
    public float fireRateSingle = 3f;
    public float fireRateAuto = 10f;

    [Header("Тепло")]
    public float heatPerShotSingle = 8f;
    public float heatPerShotAuto = 6f;
    public float maxHeat = 100f;
    public float heatDissipationRate = 25f;
    public float overheatLockoutDuration = 3f;
    public float overheatCooldownRate = 40f;

    [Header("Отдача")]
    public float recoilImpulseSingle = 6f;
    public float recoilImpulseAuto = 2f;

    [Header("Снаряд")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 40f;
    public float projectileLifetime = 3f;
}
