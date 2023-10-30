using UnityEngine;

namespace Common.Detector
{
    public class GroundDetector : MonoBehaviour, IDetector
    {
        [SerializeField]
        public bool debugger = true;
        [SerializeField]
        public LayerMask layer;
        [SerializeField]
        public Rect box;
        [SerializeField]
        public Color color;

        [SerializeField]
        private bool isOnGround;

        public bool IsOnGround => isOnGround;
        
        
        private readonly Collider2D[] _groundColliders = new Collider2D[1];
        

        public bool Detect()
        {
            var count = Physics2D.OverlapBoxNonAlloc((Vector2)transform.position + box.center,
                box.size, 0, _groundColliders, layer);
            return count > 0;
        }

        public void DrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(transform.position + (Vector3)box.center, box.size);
        }

        private void FixedUpdate()
        {
            isOnGround = Detect();
        }



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (debugger)
            {
                DrawGizmos();    
            }
        }
    }
#endif
}