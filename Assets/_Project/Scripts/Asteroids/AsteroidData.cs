using UnityEngine;

/// <summary>ScriptableObject с параметрами астероида.</summary>
[CreateAssetMenu(menuName = "Game/AsteroidData")]
public class AsteroidData : ScriptableObject
{
    public float maxHealth = 2f;
    public float speed = 6f;
    public float damageToStation = 5f;
    public int scoreReward = 10;
    public GameObject prefab;
    public GameObject explosionVFXPrefab;
}
