using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Единственный класс, который читает ввод через Input System.
/// Остальные скрипты обращаются к его свойствам, а не к Input напрямую.
/// </summary>
public class PlayerInput : MonoBehaviour
{
    private InputActions inputActions;
    private InputActions.PlayerActions playerActions;

    public Vector2 MouseScreenPosition { get; private set; }
    public bool FireHeld { get; private set; }
    public bool FirePressedThisFrame { get; private set; }
    public bool SwitchFireModePressed { get; private set; }

    void Awake()
    {
        inputActions = new InputActions();
        playerActions = inputActions.Player;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        MouseScreenPosition = playerActions.MousePosition.ReadValue<Vector2>();
        FireHeld = playerActions.Fire.IsPressed();
        FirePressedThisFrame = playerActions.Fire.WasPressedThisFrame();
        SwitchFireModePressed = playerActions.SwitchFireMode.WasPressedThisFrame();
    }
}
