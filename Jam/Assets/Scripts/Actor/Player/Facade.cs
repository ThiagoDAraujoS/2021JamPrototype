using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Actor.Player {
    public class Facade : MonoBehaviour {
        private static Facade instance;

        [SerializeField] 
        private Camera playerCamera;

        private Rigidbody2D rb;
        private Control     control;
        private Motion      motion;
        private Grapple     grapple;
        private PlayerInput playerInput;

        [SerializeField]
        private Transform
            physicsTransform;


        public void Awake() {
            if (instance != null)
                Destroy(gameObject);
            else {
                instance    = this;
                rb          = GetComponentInChildren<Rigidbody2D>();
                control     = GetComponent<Control>();
                motion      = GetComponent<Motion>();
                grapple     = GetComponentInChildren<Grapple>();
                playerInput = GetComponent<PlayerInput>();
            }
        }

        public class Managed : MonoBehaviour {
            protected static Facade      Master           => instance;
            protected static Rigidbody2D Rb               => instance.rb;
            protected static Control     Control          => instance.control;
            protected static Motion      Motion           => instance.motion;
            protected static Grapple     Grapple          => instance.grapple;
            protected static Transform   PhysicsTransform => instance.physicsTransform;
            protected static PlayerInput PlayerInput      => instance.playerInput;
            protected static Camera      PlayerCamera     => instance.playerCamera;
        }
    }
}