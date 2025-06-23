using UnityEngine;
using UnityEngine.InputSystem;
using System; // Necesario para usar Action

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;

    public Vector2 MovementValue { get; private set; }

    
    public event Action jumpStarted;
    public event Action jumpCanceled;

    private void Awake()
    {
        var map = inputActions.FindActionMap("Player");
        moveAction = map.FindAction("Move");
        jumpAction = map.FindAction("Jump");
    }

    private void OnEnable()
    {
        moveAction.started += ctx => MovementValue = ctx.ReadValue<Vector2>();
        moveAction.performed += ctx => MovementValue = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => MovementValue = Vector2.zero;

        // Disparamos los eventos cuando se inicia o cancela el salto
        jumpAction.started += ctx => jumpStarted?.Invoke();
        jumpAction.canceled += ctx => jumpCanceled?.Invoke();

        moveAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }
}