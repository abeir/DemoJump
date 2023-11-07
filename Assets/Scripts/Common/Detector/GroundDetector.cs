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

        public bool IsOnMovingPlatform { get; private set; }
        
        
        private readonly Collider2D[] _groundColliders = new Collider2D[1];
        private float _pausedTime;

        public bool Detect()
        {
            var count = Physics2D.OverlapBoxNonAlloc((Vector2)transform.position + box.center,
                box.size, 0, _groundColliders, layer);

            if (count > 0)
            {
                CheckPlatform();
                return true;
            }
            ResetPlatform();
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


        private void CheckPlatform()
        {
            switch (_groundColliders[0].tag)
            {
                case Tags.OneWayPlatform:
                    IsOnOneWayPlatform = true;
                    IsOnMovingPlatform = false;
                    break;
                case Tags.MovingPlatform:
                    IsOnOneWayPlatform = false;
                    IsOnMovingPlatform = true;
                    break;
                default:
                    ResetPlatform();
                    break;
            }
        }

        private void ResetPlatform()
        {
            IsOnOneWayPlatform = false;
            IsOnMovingPlatform = false;
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