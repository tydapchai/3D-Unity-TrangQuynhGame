using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Tydapchai.FinalCharacterController
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class ThirdPersonCameraController : MonoBehaviour
    {
        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float zoomLerpSpeed = 10f;
        [SerializeField] private float minDistance = 3f;
        [SerializeField] private float maxDistance = 15f;

        [Header("Rotation Settings")]
        [SerializeField] private float rotationSensitivity = 0.12f;
        [SerializeField] private float minPitch = -35f; // degrees (VerticalAxis)
        [SerializeField] private float maxPitch = 70f;  // degrees (VerticalAxis)

        [Header("Input")]
        [SerializeField] private bool rotateOnlyWhenRightMouseHeld = true;

        private PlayerControls controls;
        private CinemachineCamera cam;
        private CinemachineOrbitalFollow orbital;

        private float targetZoom;
        private float currentZoom;

        private void Awake()
        {
            cam = GetComponent<CinemachineCamera>();
            orbital = GetComponent<CinemachineOrbitalFollow>();

            if (orbital == null)
            {
                Debug.LogError("CinemachineOrbitalFollow not found on this CinemachineCamera.");
                enabled = false;
                return;
            }

            // init zoom
            currentZoom = orbital.Radius;
            targetZoom = currentZoom;

            // input actions (zoom)
            controls = new PlayerControls();
            controls.Enable();

            var map = controls.asset.FindActionMap("CameraControls", throwIfNotFound: false);
            if (map != null)
            {
                var zoomAction = map.FindAction("MouseZoom", throwIfNotFound: false);
                if (zoomAction != null)
                    zoomAction.performed += OnMouseZoom;
                else
                    Debug.LogWarning("Action 'MouseZoom' not found in map 'CameraControls'.");
            }
            else
            {
                Debug.LogWarning("Action map 'CameraControls' not found in PlayerControls.");
            }

            // default cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void Update()
        {
            SmoothZoom();
            HandleRotation();
        }

        private void SmoothZoom()
        {
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, zoomLerpSpeed * Time.deltaTime);
            orbital.Radius = currentZoom;
        }

        private void HandleRotation()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            bool rotating = rotateOnlyWhenRightMouseHeld ? mouse.rightButton.isPressed : true;
            if (!rotating)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }

            // lock cursor khi xoay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Vector2 delta = mouse.delta.ReadValue();

            // yaw / pitch
            orbital.HorizontalAxis.Value += delta.x * rotationSensitivity;
            orbital.VerticalAxis.Value -= delta.y * rotationSensitivity;

            // clamp pitch (VerticalAxis.Value là degrees với OrbitalFollow)
            orbital.VerticalAxis.Value = Mathf.Clamp(orbital.VerticalAxis.Value, minPitch, maxPitch);
        }

        private void OnMouseZoom(InputAction.CallbackContext context)
        {
            // scroll = Vector2, thường dùng y
            Vector2 scroll = context.ReadValue<Vector2>();
            float scrollY = scroll.y;

            // scrollY thường khá lớn/nhỏ tuỳ device -> không nhân deltaTime để cảm giác “đúng”
            targetZoom -= scrollY * zoomSpeed * 0.01f;
            targetZoom = Mathf.Clamp(targetZoom, minDistance, maxDistance);
        }

        private void OnDisable()
        {
            if (controls != null)
            {
                var map = controls.asset.FindActionMap("CameraControls", throwIfNotFound: false);
                if (map != null)
                {
                    var zoomAction = map.FindAction("MouseZoom", throwIfNotFound: false);
                    if (zoomAction != null)
                        zoomAction.performed -= OnMouseZoom;
                }

                controls.Disable();
                controls.Dispose();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}