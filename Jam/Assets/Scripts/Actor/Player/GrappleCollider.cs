using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Actor.Player {
    public class GrappleCollider : MonoBehaviour
    {        
        //[HideInInspector]
        public List<Level.Peg> pegs;

        public Level.Peg this[int index] => pegs[index];
        public List<Level.Peg> Pegs() => pegs;
        public int Count => pegs.Count;
        
        private void Awake() => pegs = new List<Level.Peg>(8);
    
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
