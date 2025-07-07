using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform pivot;
    public Vector3 offSet;
    public bool useOffsetValues;
    public float rotateSpeed;

    private void Start()
    {
        if (!useOffsetValues)
        {
            offSet = target.position - transform.position;
        }
        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, horizontal, 0);


        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        pivot.Rotate(-vertical, 0, 0);

        float desireYAngle = target.eulerAngles.y;
        float desireXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desireXAngle, desireYAngle, 0);
        transform.position = target.position - (rotation * offSet);

        //transform.position = target.position - offSet;

        transform.LookAt(target);

    }
}
