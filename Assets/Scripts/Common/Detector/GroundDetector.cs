using Common.Settings;
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

        public bool IsOnOneWayPlatform { get; private set; }
        
        
        private readonly Collider2D[] _groundColliders = new Collider2D[1];
        private float _pausedTime;

        public bool Detect()
        {
            var count = Physics2D.OverlapBoxNonAlloc((Vector2)transform.position + box.center,
                box.size, 0, _groundColliders, layer);

            if (count > 0)
            {
                IsOnOneWayPlatform = _groundColliders[0].CompareTag(Tags.OneWayPlatform);
                return true;
            }

            IsOnOneWayPlatform = false;
            return false;
        }

        public void DrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(transform.position + (Vector3)box.center, box.size);
        }

        public void Pause(float t)
        {
            _pausedTime = t;
        }

        public void Resume()
        {
            _pausedTime = 0;
        }

        private void FixedUpdate()
        {
            if (_pausedTime > 0)
            {
                isOnGround = false;
                _pausedTime -= Time.fixedDeltaTime;
                return;
            }
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