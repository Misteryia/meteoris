using UnityEngine;

/// <summary>
/// Снаряд турели. Летит по прямой, при попадании в астероид наносит урон и возвращается в пул.
/// На префабе: Rigidbody (useGravity=false) + SphereCollider (isTrigger=true).
/// </summary>
public class Projectile : MonoBehaviour
{
    public float Damage { get; set; }

    private Rigidbody rb;
    private ProjectilePool pool;
    private float lifetimeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>Задаёт ссылку на пул для возврата.</summary>
    public void SetPool(ProjectilePool ownerPool)
    {
        pool = ownerPool;
    }

    /// <summary>Запускает снаряд в указанном направлении.</summary>
    public void Launch(Vector3 direction, float speed, float lifetime)
    {
        lifetimeTimer = lifetime;
        transform.forward = direction.normalized;
        rb.linearVelocity = direction.normalized * speed;
    }

    void Update()
    {
        lifetimeTimer -= Time.deltaTime;
        if (lifetimeTimer <= 0f)
            ReturnToPool();
    }

    void OnTriggerEnter(Collider other)
    {
        Asteroid asteroid = other.GetComponent<Asteroid>();
        if (asteroid != null)
        {
            asteroid.TakeDamage(Damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;
        if (pool != null)
            pool.Return(this);
        else
            gameObject.SetActive(false);
    }
}
