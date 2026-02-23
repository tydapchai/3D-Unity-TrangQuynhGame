using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tydapchai.FinalCharacterController
{
    public class PlayerLocomotionInput : MonoBehaviour
    {
        // This script is no longer used. Movement is handled directly in PlayerController using legacy Input.
        // Keeping this file for compatibility, but it can be removed if not needed.

        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private PlayerInput playerInput;

        private void Awake()
        {
            // No longer using PlayerInput
        }

        private void OnEnable()
        {
            // Disabled
        }

        private void OnDisable()
        {
            // Disabled
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            // Disabled
        }

        private void OnLook(InputAction.CallbackContext context)
        {
            // Disabled
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            // Disabled
        }

        private void OnSprint(InputAction.CallbackContext context)
        {
            // Disabled
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            // Disabled
        }

        private void SetCursorState(bool newState)
        {
            // Disabled
        }
    }
}

