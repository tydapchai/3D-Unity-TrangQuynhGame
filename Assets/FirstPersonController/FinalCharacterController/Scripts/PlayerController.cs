using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tydapchai.FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Components")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform cameraTransform;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -9.81f;

        [Header("Camera Settings")]
        [SerializeField] private float mouseSensitivity = 100f;
        [SerializeField] private float maxLookAngle = 90f;

        private Vector3 velocity;
        private float xRotation = 0f;

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction sprintAction;

        private void Awake()
        {
            if (controller == null) controller = GetComponent<CharacterController>();
            if (cameraTransform == null) cameraTransform = Camera.main.transform;

            // Setup input actions
            moveAction = new InputAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            moveAction.Enable();

            lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
            lookAction.Enable();

            jumpAction = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
            jumpAction.Enable();

            sprintAction = new InputAction("Sprint", InputActionType.Button, "<Keyboard>/leftShift");
            sprintAction.Enable();

            // Lock cursor for first-person view
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleMovement();
            HandleCamera();
        }

        private void HandleMovement()
        {
            // Ground check
            bool isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Small negative value to keep grounded
            }

            // Get input from WASD
            Vector2 moveInput = moveAction.ReadValue<Vector2>();
            float horizontal = moveInput.x; // A/D
            float vertical = moveInput.y; // W/S

            // Determine speed
            float speed = sprintAction.IsPressed() ? sprintSpeed : moveSpeed;

            // Calculate movement direction
            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            controller.Move(move * speed * Time.deltaTime);

            // Jump
            if (jumpAction.WasPressedThisFrame() && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void HandleCamera()
        {
            // Mouse look
            Vector2 lookInput = lookAction.ReadValue<Vector2>();
            float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        private void OnDestroy()
        {
            // Disable actions
            moveAction.Disable();
            lookAction.Disable();
            jumpAction.Disable();
            sprintAction.Disable();
        }
    }
}
