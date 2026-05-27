using UnityEngine;

/// <summary>
/// Поворачивает дуло турели в сторону курсора мыши.
/// Прицеливание через Raycast от камеры на плоскость Y=0.
/// </summary>
public class TurretAimer : MonoBehaviour
{
    [SerializeField] private Transform barrel;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundMask;

    private PlayerInput playerInput;

    /// <summary>Нормализованный вектор направления в плоскости XZ.</summary>
    public Vector3 AimDirection { get; private set; }

    /// <summary>Мировая позиция точки вылета снаряда.</summary>
    public Vector3 MuzzleWorldPosition { get; private set; }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState != GameState.Playing)
            return;

        Ray ray = mainCamera.ScreenPointToRay(playerInput.MouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundMask))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;

            Vector3 direction = (target - transform.position).normalized;

            if (direction.sqrMagnitude > 0.001f)
            {
                AimDirection = direction;

                if (barrel != null)
                    barrel.forward = AimDirection;
            }
        }

        if (barrel != null)
        {
            Transform muzzle = barrel.Find("MuzzlePoint");
            MuzzleWorldPosition = muzzle != null ? muzzle.position : barrel.position;
        }
    }
}
