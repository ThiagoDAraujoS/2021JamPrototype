using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor.Player {
    public class Whip : Facade.Managed {
        private List<Level.Peg> pegs;
        private Vector2         direction = Vector2.zero;
        private bool            isFiring;

        [SerializeField]                 private AnimationCurve focusDistanceStrengthCurve;
        [SerializeField]                 private AnimationCurve whipDistanceStrengthCurve;
        [SerializeField] [Range(0f, 1f)] private float          focusOverWhipDistance = 0.5f;
        public void Awake() {
            pegs = new List<Level.Peg>(8);
            StartCoroutine(FireRoutine());
        }

        private IEnumerator FireRoutine() {
            // ReSharper disable once TooWideLocalVariableScope
            Level.Peg target;
            
            while (true) {
                yield return new WaitUntil(() => isFiring);

                //Rotate the whip collider to match the direction of the whip
                WhipCollider.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f);

                //wait a frame so the physics engine has time to update the collider
                yield return new WaitForFixedUpdate();
                
                //get the peg that is the most probable to hit
                target = CalculatePeg();

                if (target != null)
                    BuildWhip(target);

                //Clear the direction
                isFiring = false;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        /// <summary>
        /// Evaluate all the pegs and return the one that is the most likely to be hit
        /// </summary>
        private Level.Peg CalculatePeg() {
            Level.Peg result   = null;
            float     maxValue = float.MinValue;

            //For each peg in the hit box
            foreach (Level.Peg peg in pegs) {
                Vector3 whipPos   = WhipCollider.position,
                        whipScale = WhipCollider.localScale,
                        pegPos    = peg.transform.position;

                //Calculate closeness to whip focus line
                Vector3 focusPos =
                    Quaternion.Euler(0, 0, -Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f) *
                    (pegPos - whipPos);

                //Transform distance into percentage then tone the value map using a spline
                float focusDist = focusDistanceStrengthCurve.Evaluate(
                    Mathf.Clamp01(1f - Mathf.Abs(focusPos.x) / (focusPos.y / (whipScale.y / whipScale.x))));

                //Calculate the distance to the peg then transform the value into a percentage tone the value map using a spline
                float whipDist =
                    whipDistanceStrengthCurve.Evaluate(Mathf.Clamp01(1f - focusPos.y / whipScale.y * 0.5f));

                //Interpolate whip distance from focus distance
                float value = Mathf.Lerp(focusDist, whipDist, focusOverWhipDistance);

                //Save peg if this is the more likely to be hit
                if (!(value > maxValue)) continue;
                maxValue = value;
                result   = peg;

                Debug.Log("Focus = " + focusDist + " Distance = " + whipDist + "Value = " + value);
            }

            return result;
        }


        private void OnDrawGizmos() {
            if (pegs == null) return;
            
            Gizmos.color = Color.red;
            Level.Peg target = CalculatePeg();
            if (target == null) return;
            Gizmos.DrawSphere(target.transform.position, 0.1f);
        }

        private void BuildWhip(Level.Peg target) {
            
        }

        public void Fire(Vector2 direction) {
            isFiring       = true;
            this.direction = direction;
        }

        public void Sling() { }

        private void OnTriggerEnter2D(Collider2D other) {
            Level.Peg peg = other.GetComponent<Level.Peg>();
            if (peg != null && !pegs.Contains(peg))
                pegs.Add(peg);
        }

        private void OnTriggerExit2D(Collider2D other) {
            Level.Peg peg = other.GetComponent<Level.Peg>();
            if (peg != null && pegs.Contains(peg))
                pegs.Remove(peg);
        }
    }
}
