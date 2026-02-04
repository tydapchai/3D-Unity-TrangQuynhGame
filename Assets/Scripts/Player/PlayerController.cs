using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("[PlayerController] Rigidbody not found on Player!");
        }
        else
        {
            Debug.Log("[PlayerController] Rigidbody found and initialized");
        }
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();

        // DEBUG: Print velocity
        if (moveDirection.magnitude > 0)
        {
            Debug.Log($"[PlayerController] Velocity: {rb.linearVelocity}, MoveDir: {moveDirection}");
        }
    }

    private void HandleInput()
    {
        // Get movement input from keyboard
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.wKey.isPressed)
            vertical += 1f;
        if (Keyboard.current.sKey.isPressed)
            vertical -= 1f;
        if (Keyboard.current.aKey.isPressed)
            horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed)
            horizontal += 1f;

        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    private void Move()
    {
        if (rb == null)
        {
            Debug.LogError("[PlayerController] Rigidbody is null!");
            return;
        }

        if (moveDirection.magnitude > 0)
        {
            // Move player
            Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

            // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            Debug.Log($"[PlayerController] Moving: {moveDirection}");
        }
        else
        {
            // Stop horizontal movement
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
}
