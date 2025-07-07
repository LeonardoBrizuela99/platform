using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float height = 2.0f;
    public float rotationSpeed = 2.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 2.0f;

    private float _currentRotationAngle;
    private float _desiredRotationAngle;
    private float _mouseX;
    private float _mouseY;
    private Vector3 _offset;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target is not assigned for the third-person camera.");
            enabled = false;
            return;
        }

        _offset = new Vector3(0, height, -distance);
        _currentRotationAngle = transform.eulerAngles.y;
        _desiredRotationAngle = _currentRotationAngle;

        UpdateCameraPosition();
    }

    void LateUpdate()
    {
        HandleInput();
        UpdateCameraPosition();
    }

    void HandleInput()
    {
        _mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        _mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        _mouseY = Mathf.Clamp(_mouseY, -60f, 60f); 

        _desiredRotationAngle = _mouseX;
    }


    void UpdateCameraPosition()
    {
        if (target == null) return;

       
        _currentRotationAngle = Mathf.LerpAngle(_currentRotationAngle, _desiredRotationAngle, rotationDamping * Time.deltaTime);

        
        Quaternion currentRotation = Quaternion.Euler(0, _currentRotationAngle, 0);
        Vector3 desiredPosition = target.position + currentRotation * _offset;

        
        float currentHeight = Mathf.Lerp(transform.position.y, target.position.y + height, heightDamping * Time.deltaTime);

       
        transform.position = desiredPosition;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);


      
        transform.LookAt(target.position + Vector3.up * height / 2);
    }
}
