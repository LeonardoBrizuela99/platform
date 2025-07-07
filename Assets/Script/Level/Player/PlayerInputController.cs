using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }

    public event Action OnJumpButtonPressed;

    private void OnMove(InputValue inputValue)
    {
        
        Vector2 input = inputValue.Get<Vector2>();
      
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

       
        Vector3 worldMovement = camForward * input.y + camRight * input.x;

       
        MovementInputVector = new Vector2(worldMovement.x, worldMovement.z);
    }

    private void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed)
            OnJumpButtonPressed?.Invoke();
    }
}
