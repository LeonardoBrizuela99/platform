using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;

    [Header("Jump Force Settings")]
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxFirstJumpForce = 12f;
    [SerializeField] private float maxSecondJumpForce = 8f;
    [SerializeField] private float chargeSpeed = 15f;

    [Header("Jump Limits & Ground Check")]
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private bool isCharging;
    private float currentJumpForce;
    private int jumpCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputReader.jumpStarted += StartCharging;
        inputReader.jumpCanceled += PerformJump;
    }

    private void OnDisable()
    {
        inputReader.jumpStarted -= StartCharging;
        inputReader.jumpCanceled -= PerformJump;
    }

    private void Update()
    {

        if (isCharging && jumpCount < maxJumpCount)
        {
            currentJumpForce += chargeSpeed * Time.deltaTime;
            float maxForce = (jumpCount == 0) ? maxFirstJumpForce : maxSecondJumpForce;
            currentJumpForce = Mathf.Clamp(currentJumpForce, minJumpForce, maxForce);
        }
    }

    private void StartCharging()
    {

        if (jumpCount < maxJumpCount)
        {
            isCharging = true;
            currentJumpForce = minJumpForce;
        }
    }

    private void PerformJump()
    {

        if (isCharging && jumpCount < maxJumpCount)
        {
            float appliedForce = Mathf.Clamp(currentJumpForce, minJumpForce, (jumpCount == 0) ? maxFirstJumpForce : maxSecondJumpForce);
            rb.AddForce(Vector3.up * appliedForce, ForceMode.Impulse);
            jumpCount++;
        }
        isCharging = false;
        currentJumpForce = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (((1 << collision.gameObject.layer) & groundMask) != 0)
        {
            jumpCount = 0;
        }
    }
}