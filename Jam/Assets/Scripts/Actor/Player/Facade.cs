using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player {
    public class Facade : MonoBehaviour {
        public static Rigidbody2D Rb               => Instance.rb;
        public static Control     Control          => Instance.control;
        public static Motion      Motion           => Instance.motion;
        public static Whip        Whip             => Instance.whip;
        public static Transform   PhysicsTransform => Instance.physicsTransform;
        public static PlayerInput PlayerInput      => Instance.playerInput;
        public static Camera      PlayerCamera     => Instance.playerCamera;


        //--------------------------------------------------------------------------------------------------------------
        private static Facade Instance;

        [SerializeField] private Camera playerCamera;

        private Rigidbody2D rb;
        private Control     control;
        private Motion      motion;
        private Whip        whip;
        private Transform   physicsTransform;
        private PlayerInput playerInput;

        public void Awake() {
            if (Instance != null)
                Destroy(gameObject);
            else {
                Instance    = this;
                rb          = GetComponent<Rigidbody2D>();
                control     = GetComponent<Control>();
                motion      = GetComponent<Motion>();
                whip        = GetComponent<Whip>();
                playerInput = GetComponent<PlayerInput>();

                physicsTransform = transform.Find("Physics");
            }
        }

        public class Managed : MonoBehaviour {
            public static    Facade      Master           => Instance;
            protected static Rigidbody2D Rb               => Facade.Rb;
            protected static Control     Control          => Facade.Control;
            protected static Motion      Motion           => Facade.Motion;
            protected static Whip        Whip             => Facade.Whip;
            protected static Transform   PhysicsTransform => Facade.PhysicsTransform;
            protected static PlayerInput PlayerInput      => Facade.PlayerInput;
            protected static Camera      PlayerCamera     => Facade.PlayerCamera;
        }
    }
}