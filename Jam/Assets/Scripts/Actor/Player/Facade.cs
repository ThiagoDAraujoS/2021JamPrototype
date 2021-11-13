using UnityEngine;

namespace Actor.Player {
    public class Facade : MonoBehaviour {
        public static Rigidbody2D Rb               => Instance.rb;
        public static Control     Control          => Instance.control;
        public static Motion      Motion           => Instance.motion;
        public static Transform   PhysicsTransform => Instance.physicsTransform;
        
        
        //--------------------------------------------------------------------------------------------------------------
        private static Facade Instance;

        private Rigidbody2D rb;
        private Control     control;
        private Motion      motion;
        private Transform   physicsTransform;
        
        public void Awake() {
            if (Instance != null)
                Destroy(gameObject);
            else {
                Instance = this;
                rb       = GetComponent<Rigidbody2D>();
                control  = GetComponent<Control>();
                motion   = GetComponent<Motion>();

                physicsTransform = transform.Find("Physics");
            }
        }

        public class Managed : MonoBehaviour {
            public static    Facade      Master           => Instance;
            protected static Rigidbody2D Rb               => Facade.Rb;
            protected static Control     Control          => Facade.Control;
            protected static Motion      Motion           => Facade.Motion;
            protected static Transform   PhysicsTransform => Facade.PhysicsTransform;
        }
    }


}