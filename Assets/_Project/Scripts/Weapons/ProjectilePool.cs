using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Простой пул снарядов на List. Переиспользует деактивированные объекты.
/// </summary>
public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int initialSize = 30;

    private List<Projectile> pool = new List<Projectile>();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
            CreateProjectile();
    }

    public Projectile Get()
    {
        foreach (var p in pool)
        {
            if (!p.gameObject.activeInHierarchy)
            {
                p.gameObject.SetActive(true);
                return p;
            }
        }

        return CreateProjectile();
    }

    public void Return(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private Projectile CreateProjectile()
    {
        Projectile p = Instantiate(projectilePrefab, transform);
        p.SetPool(this);
        p.gameObject.SetActive(false);
        pool.Add(p);
        return p;
    }
}
