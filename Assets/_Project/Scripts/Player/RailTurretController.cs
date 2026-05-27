using UnityEngine;

/// <summary>
/// Двигает турель по круговой рельсе вокруг станции.
/// Перемещение только через recoil от выстрелов — WASD не используется.
/// </summary>
public class RailTurretController : MonoBehaviour
{
    [SerializeField] private Transform stationCenter;
    [SerializeField] private float railRadius = 8f;
    [SerializeField] private float railDamping = 0.95f;
    [SerializeField] private float maxRailSpeed = 12f;

    /// <summary>Текущий угол на окружности (в радианах).</summary>
    public float CurrentAngleRad { get; private set; }

    /// <summary>Тангенциальная скорость по рельсе (положительная = против часовой).</summary>
    public float TangentialSpeed { get; private set; }

    void Start()
    {
        // Вычисляем начальный угол из текущей позиции турели
        Vector3 offset = transform.position - stationCenter.position;
        CurrentAngleRad = Mathf.Atan2(offset.z, offset.x);
    }

    void FixedUpdate()
    {
        // Затухание скорости
        TangentialSpeed *= railDamping;

        // Ограничение максимальной скорости
        TangentialSpeed = Mathf.Clamp(TangentialSpeed, -maxRailSpeed, maxRailSpeed);

        // Обновляем угол: линейная скорость = угловая * радиус, значит угловая = линейная / радиус
        CurrentAngleRad += TangentialSpeed / railRadius * Time.fixedDeltaTime;

        // Пересчитываем мировую позицию на окружности
        Vector3 center = stationCenter.position;
        Vector3 newPos = new Vector3(
            center.x + Mathf.Cos(CurrentAngleRad) * railRadius,
            center.y,
            center.z + Mathf.Sin(CurrentAngleRad) * railRadius
        );

        transform.position = newPos;
    }

    /// <summary>
    /// Применяет recoil-импульс при выстреле.
    /// Берёт тангенциальную составляющую направления выстрела и толкает турель в противоположную сторону.
    /// </summary>
    /// <param name="fireDirection">Направление выстрела в мировых координатах.</param>
    /// <param name="impulse">Сила импульса.</param>
    public void ApplyRecoilImpulse(Vector3 fireDirection, float impulse)
    {
        // Тангенциальный вектор в текущей точке окружности (против часовой стрелки)
        Vector3 tangent = new Vector3(
            -Mathf.Sin(CurrentAngleRad),
            0f,
            Mathf.Cos(CurrentAngleRad)
        );

        // Проецируем направление выстрела на тангенциальный вектор
        float tangentialComponent = Vector3.Dot(fireDirection.normalized, tangent);

        // Толкаем в противоположную сторону от выстрела
        TangentialSpeed -= tangentialComponent * impulse;
    }
}
