using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Detector
{
    public class GroundDetector : MonoBehaviour, IDetector
    {
        [SerializeField]
        public bool debugger = true;
        [SerializeField]
        public LayerMask groundLayer;
        [SerializeField]
        public Rect detectBox;
        [SerializeField]
        public Color detectBoxColor;
        
        
        public bool IsOnGround { get; private set; }
        
        
        private readonly Collider2D[] _groundColliders = new Collider2D[1];
        

        public bool Detect()
        {
            var count = Physics2D.OverlapBoxNonAlloc((Vector2)transform.position + detectBox.center, 
                    detectBox.size, 0, _groundColliders, groundLayer);
            return count > 0;
        }

        public void DrawGizmos()
        {
            Gizmos.color = detectBoxColor;
            Gizmos.DrawWireCube(transform.position + (Vector3)detectBox.center, detectBox.size);
        }

        private void FixedUpdate()
        {
            IsOnGround = Detect();
        }

        private void OnDrawGizmos()
        {
            if (debugger)
            {
                DrawGizmos();    
            }
        }
    }
}