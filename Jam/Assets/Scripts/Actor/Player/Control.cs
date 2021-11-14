using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

namespace Actor.Player {

    public class Control : Facade.Managed {
        public enum ControlSchemeEnum {
            KeyboardAndMouse,
            Gamepad
        }

        private InputMap inputMap;

        [SerializeField] private ControlSchemeEnum controlScheme;

        public ControlSchemeEnum ControlScheme {
            get => controlScheme;
            set {
                if (controlScheme == value) return;
                inputMap.Player.WhipAim.performed -= controlScheme switch {
                    ControlSchemeEnum.KeyboardAndMouse => OnWhipAimMouse,
                    ControlSchemeEnum.Gamepad          => OnWhipAimGamepad,
                    _                                  => throw new ArgumentOutOfRangeException()
                };
                controlScheme = value;
                inputMap.Player.WhipAim.performed += controlScheme switch {
                    ControlSchemeEnum.KeyboardAndMouse => OnWhipAimMouse,
                    ControlSchemeEnum.Gamepad          => OnWhipAimGamepad,
                    _                                  => throw new ArgumentOutOfRangeException()
                };
            }
        }

        private float   moveAxis;
        private Vector2 whipAim;


        private void OnWalk(InputAction.CallbackContext  ctx) => moveAxis = ctx.ReadValue<float>();
        private void OnStop(InputAction.CallbackContext  ctx) => moveAxis = 0.0f;
        private void OnJump(InputAction.CallbackContext  ctx) => Motion.Jump();
        private void OnBoost(InputAction.CallbackContext ctx) => Motion.Boost();
        private void OnSling(InputAction.CallbackContext ctx) => Whip.Sling();
        private void OnWhip(InputAction.CallbackContext  ctx) => Whip.Fire(whipAim);

        public void OnWhipAimMouse(InputAction.CallbackContext ctx) {
            Vector2 mousePosition = ctx.ReadValue<Vector2>();
            Debug.Assert(Camera.main != null, "Camera.main != null");
            whipAim = (mousePosition - (Vector2)Camera.main.WorldToScreenPoint(PhysicsTransform.position)).normalized;
        }

        public void OnWhipAimGamepad(InputAction.CallbackContext ctx) {
            whipAim = ctx.ReadValue<Vector2>();
        }


        public void Awake() {
            inputMap = new InputMap();
        }

        private void Update() {
            if (moveAxis != 0)
                Motion.Move(moveAxis);
        }

        private void OnEnable() {
            inputMap.Player.Walk.performed  += OnWalk;
            inputMap.Player.Walk.canceled   += OnStop;
            inputMap.Player.Jump.performed  += OnJump;
            inputMap.Player.Boost.performed += OnBoost;
            inputMap.Player.Sling.performed += OnSling;
            inputMap.Player.Whip.performed  += OnWhip;
            inputMap.Player.WhipAim.performed += controlScheme switch {
                ControlSchemeEnum.KeyboardAndMouse => OnWhipAimMouse,
                ControlSchemeEnum.Gamepad          => OnWhipAimGamepad,
                _                                  => throw new ArgumentOutOfRangeException()
            };
        }

        private void OnDisable() {
            inputMap.Player.Walk.performed  -= OnWalk;
            inputMap.Player.Walk.canceled   -= OnStop;
            inputMap.Player.Jump.performed  -= OnJump;
            inputMap.Player.Boost.performed -= OnBoost;
            inputMap.Player.Sling.performed -= OnSling;
            inputMap.Player.Whip.performed  -= OnWhip;
            inputMap.Player.WhipAim.performed += controlScheme switch {
                ControlSchemeEnum.KeyboardAndMouse => OnWhipAimMouse,
                ControlSchemeEnum.Gamepad          => OnWhipAimGamepad,
                _                                  => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
