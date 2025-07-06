using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _jumpSpeed;
    private PlayerInputController playerInputController;
    private GroundController groundController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;

    private float jumpButtonGracePeriod = 0.2f;
    private float? lastGroundedTime = null;
    private float? jumpButtonPressedTime = null;

    private void Update()
    {
        if (groundController.IsGrounded)
        {
            lastGroundedTime = Time.time;
        }

    }

    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        _rigidbody = GetComponent<Rigidbody>();
        groundController = GetComponent<GroundController>();
        playerInputController.OnJumpButtonPressed += JumpButtonPressed;
        playerInputController.OnJumpButtonPressed += RecordJumpPressedTime;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(playerInputController.MovementInputVector.x, 0, playerInputController.MovementInputVector.y) * _speed;

        velocity.y = _rigidbody.linearVelocity.y;

        if (jumpButtonPressedTime.HasValue)
        {
            bool withinJumpTime = (Time.time - jumpButtonPressedTime.Value) <= jumpButtonGracePeriod;
            bool withinGroundTime = lastGroundedTime.HasValue
                && (Time.time - lastGroundedTime.Value) <= jumpButtonGracePeriod;

            if (withinJumpTime && withinGroundTime)
            {
                _jumpTriggered = true;
                jumpButtonPressedTime = null;
            }
        }

        if (_jumpTriggered)
        {
            velocity.y = _jumpSpeed;
            _jumpTriggered = false;
        }
        _rigidbody.linearVelocity = velocity;
    }

    private void JumpButtonPressed()
    {
        if (groundController.IsGrounded)
        {
            _jumpTriggered = true;
        }
    }

    private void RecordJumpPressedTime()
    {
        
        jumpButtonPressedTime = Time.time;
    }
}
