using UnityEngine;

namespace Actor.Player {
    public class Motion : Facade.Managed {
        [SerializeField] private float
            walkSpeedLimit    = 5.0f,
            walkSpeedScale    = 1.0f,
            jumpForce         = 1.0f,
            groundingDistance = 0.1f,
            groundingSpacing  = 0.1f;

        public void Move(float direction) {
            Vector2 directionVector = new Vector2(direction, 0);

            float speed = Mathf.Clamp(walkSpeedLimit - Mathf.Abs(Rb.velocity.x), 0f, walkSpeedLimit) * walkSpeedScale;

            if (speed > 0)
                Rb.AddForce(directionVector * speed, ForceMode2D.Force);
        }

        public void Jump() {
            if (IsGrounded)
                Rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        public void Boost() {
            
        }
        
        private bool IsGrounded {
            get {
                RaycastHit2D hit = Physics2D.Raycast(
                    PhysicsTransform.position + Vector3.left * groundingSpacing,
                    Vector2.down, groundingDistance, 
                    LayerMask.GetMask("Ground"));
                if (hit.collider != null) return true;

                hit = Physics2D.Raycast(
                    PhysicsTransform.position + Vector3.right * groundingSpacing, 
                    Vector2.down, groundingDistance, 
                    LayerMask.GetMask("Ground"));
                return hit.collider != null;
            }
        }

        private void OnDrawGizmos() {
            if (Master == null) return;
            Gizmos.color = Color.green;
            Vector3 position = PhysicsTransform.position;
            Gizmos.DrawRay(position + Vector3.left  * groundingSpacing, Vector3.down * groundingDistance);
            Gizmos.DrawRay(position + Vector3.right * groundingSpacing, Vector3.down * groundingDistance);
        }
        
    }
}
