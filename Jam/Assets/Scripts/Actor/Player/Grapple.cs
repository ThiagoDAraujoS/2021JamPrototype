using System.Collections;
using UnityEngine;

namespace Actor.Player {
    public class Grapple : Facade.Managed {

        [SerializeField]
        private AnimationCurve
            focusDistanceStrengthCurve,
            grappleDistanceStrengthCurve;

        [SerializeField] [Range(0f, 1f)]
        private float focusOverGrappleDistance = 0.5f;

        [SerializeField]
        private GrappleCollider triggerCollider;

        [SerializeField]
        private float distance = 0.0f;
        
        private SpringJoint2D springJoint;

        private void Awake() => springJoint = GetComponentInChildren<SpringJoint2D>();
        
        private IEnumerator FireRoutine(Vector2 direction) {
            triggerCollider.gameObject.SetActive(true);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ;

            //Rotate the grapple collider to match the direction of the grapple
            triggerCollider.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);

            //wait a frame so the physics engine has time to update the collider
            yield return new WaitForFixedUpdate();

            //get the peg that is the most probable to hit
            Level.Peg target = CalculatePegValue(angle);


            if (target != null) {
                
#if UNITY_EDITOR
                gizmoDrawingPos = target.transform.position;
#endif
                
                BuildGrapple(target);
            }
            triggerCollider.gameObject.SetActive(false);
        }

        private Level.Peg CalculatePegValue(float angle) {
            Level.Peg result    = null;
            float     maxValue  = float.MinValue;
            Vector3   grapplePos   = triggerCollider.transform.position;
            Vector3   grappleScale = triggerCollider.transform.localScale;

#if UNITY_EDITOR
            string debug = "";
#endif

            //For each peg in the hit box
            foreach (Level.Peg peg in triggerCollider.pegs) {
                Vector3 pegPos = peg.transform.position;

                //Calculate closeness to grapple focus line
                Vector3 focusPos = Quaternion.Euler(0, 0, -angle + 90f) * (pegPos - grapplePos);

                //Transform distance into percentage then tone the value map using a spline
                float focusDist = focusDistanceStrengthCurve.Evaluate(
                    Mathf.Clamp01(1f - Mathf.Abs(focusPos.x) / (focusPos.y / (grappleScale.y / grappleScale.x))));

                //Calculate the distance to the peg then transform the value into a percentage tone the value map using a spline
                float pegDist = grappleDistanceStrengthCurve.Evaluate(
                    Mathf.Clamp01(1f - focusPos.y / grappleScale.y * 0.5f));

                //Interpolate grapple distance from focus distance
                float value = Mathf.Lerp(focusDist, pegDist, focusOverGrappleDistance);

#if UNITY_EDITOR
                debug +=
                    peg.name       +
                    " Focus = "    + $"{focusDist:0.00}" +
                    " Distance = " + $"{pegDist:0.00}"  +
                    " Value = "    + $"{value:0.00}"     + "\n";
#endif

                //Save peg if this is the more likely to be hit
                if (!(value > maxValue)) continue;
                maxValue = value;
                result   = peg;
                
            }

#if UNITY_EDITOR
            Debug.Log(debug);
#endif

            return result;
        }

        private void BuildGrapple(Level.Peg target) {
            springJoint.connectedAnchor = target.transform.position;
            
            
            
            
        }
        
        private void ReleaseGrapple() {
            
            
            
            
        }

        public void Fire(Vector2 direction) {
            StartCoroutine(FireRoutine(direction));
        }

        public void Sling() { }
        
        
        
#if UNITY_EDITOR
        private Vector3 gizmoDrawingPos = Vector3.zero;

        private void OnDrawGizmos() {
            if (gizmoDrawingPos == Vector3.zero) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gizmoDrawingPos, 0.05f);
        }
#endif     
        
    }
}
