using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player {
    
    public class Control : Facade.Managed
    {
        private InputMap inputMap;
        private Vector2  movement;
        
        private void Walk(InputAction.CallbackContext ctx) {
            movement = ctx.ReadValue<Vector2>();
        }
        private void Stop(InputAction.CallbackContext ctx) {
            movement = Vector2.zero;
        }
        
        private void Awake() {
            inputMap = new InputMap();
        }

        private void Update() {
            if(movement.magnitude > 0)
                Motion.Move(movement.x);
        }

        private void OnEnable() {
            inputMap.Enable();
            inputMap.Player.Walk.performed += Walk;
            inputMap.Player.Walk.canceled  += Stop;
        }

        private void OnDisable() {
            inputMap.Player.Walk.performed -= Walk;
            inputMap.Player.Walk.canceled  -= Stop;
            inputMap.Disable();
        }


    }
}
