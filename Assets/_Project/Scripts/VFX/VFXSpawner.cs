using UnityEngine;

/// <summary>
/// Утилита для спавна VFX эффектов с автоуничтожением.
/// </summary>
public static class VFXSpawner
{
    /// <summary>Спавнит VFX префаб и уничтожает его через заданное время.</summary>
    public static void Spawn(GameObject vfxPrefab, Vector3 position, float lifetime = 1.8f)
    {
        if (vfxPrefab == null) return;
        GameObject go = Object.Instantiate(vfxPrefab, position, Quaternion.identity);
        Object.Destroy(go, lifetime);
    }
}
