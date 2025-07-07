using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement & Jump")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;

    private Vector3 moveDirection;
    //[Header("Orientation (assign your camera or an empty forward‐facing object here)")]
    //[SerializeField] private Transform orientation;

    private PlayerInputController playerInputController;
    private GroundController groundController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;

    [Header("Double Jump & Coyote Time")]
    [SerializeField] private int _maxJumps = 2;
    private int _jumpsRemaining;
    private bool _wasGrounded;
    private float jumpButtonGracePeriod = 0.2f;
    private float? lastGroundedTime = null;
    private float? jumpButtonPressedTime = null;


    private void Awake()
    {

        _jumpsRemaining = _maxJumps;
        _wasGrounded = false;

        playerInputController = GetComponent<PlayerInputController>();
        _rigidbody = GetComponent<Rigidbody>();
        groundController = GetComponent<GroundController>();
        playerInputController.OnJumpButtonPressed += JumpButtonPressed;
        playerInputController.OnJumpButtonPressed += RecordJumpPressedTime;
    }

    private void Update()
    {
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection=moveDirection.normalized *_speed;
        bool isGrounded = groundController.IsGrounded;

        if (isGrounded && !_wasGrounded)
            _jumpsRemaining = _maxJumps;

        if (isGrounded)
            lastGroundedTime = Time.time;

        _wasGrounded = isGrounded;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(
         playerInputController.MovementInputVector.x,
         0,
         playerInputController.MovementInputVector.y
     ) * _speed;

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
            _jumpsRemaining--;
        }

        _rigidbody.linearVelocity = velocity;
    }

    private void JumpButtonPressed()
    {
        if (groundController.IsGrounded || _jumpsRemaining > 0)
            _jumpTriggered = true;
    }

    private void RecordJumpPressedTime()
    {
        jumpButtonPressedTime = Time.time;
    }

   
}
