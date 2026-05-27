using UnityEngine;

/// <summary>
/// Рисует визуальное кольцо рельсы через LineRenderer.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class RailVisual : MonoBehaviour
{
    [SerializeField] private float radius = 8f;
    [SerializeField] private int segments = 64;

    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.positionCount = segments + 1;
        lr.useWorldSpace = false;
        lr.loop = true;

        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * Mathf.PI * 2f;
            Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            lr.SetPosition(i, pos);
        }
    }
}
