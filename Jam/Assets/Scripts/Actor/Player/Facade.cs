using UnityEngine;
using UnityEngine.InputSystem;
// ReSharper disable MemberHidesStaticFromOuterClass

namespace Actor.Player {
    public class Facade : MonoBehaviour {
        private static Facade instance;

        [SerializeField] private Camera playerCamera;

        private Rigidbody2D rb;
        private Control     control;
        private Motion      motion;
        private Whip        whip;
        private PlayerInput playerInput;

        [SerializeField] 
        private Transform
            physicsTransform,
            whipCollider;
        

        public void Awake() {
            if (instance != null)
                Destroy(gameObject);
            else {
                instance    = this;
                rb          = GetComponentInChildren<Rigidbody2D>();
                control     = GetComponent<Control>();
                motion      = GetComponent<Motion>();
                whip        = GetComponentInChildren<Whip>();
                playerInput = GetComponent<PlayerInput>();
            }
        }

        public class Managed : MonoBehaviour {
            public static    Facade      Master           => instance;
            protected static Rigidbody2D Rb               => instance.rb;
            protected static Control     Control          => instance.control;
            protected static Motion      Motion           => instance.motion;
            protected static Whip        Whip             => instance.whip;
            protected static Transform   PhysicsTransform => instance.physicsTransform;
            protected static PlayerInput PlayerInput      => instance.playerInput;
            protected static Camera      PlayerCamera     => instance.playerCamera;
            protected static Transform   WhipCollider     => instance.whipCollider;
        }
    }
}