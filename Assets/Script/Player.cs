using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class PLayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private InputReader inputReader;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
    }

    private void Update()
    {
        Vector2 input = inputReader.MovementValue;
        Vector3 move = new Vector3(input.x, 0, input.y);
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}