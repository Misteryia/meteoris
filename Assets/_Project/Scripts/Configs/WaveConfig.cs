using UnityEngine;

/// <summary>ScriptableObject описывающий одну волну спавна астероидов.</summary>
[CreateAssetMenu(menuName = "Game/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    public float duration = 30f;
    public int totalAsteroids = 10;
    public float spawnInterval = 2.5f;
    [Range(0, 1)] public float smallChance = 1f;
    [Range(0, 1)] public float mediumChance = 0f;
    [Range(0, 1)] public float largeChance = 0f;
    public bool isFinalWave = false;
}
