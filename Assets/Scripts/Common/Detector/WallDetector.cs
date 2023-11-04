using Common.Settings;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Detector
{
    public class WallDetector : MonoBehaviour, IDetector
    {
        [SerializeField]
        public bool debugger = true;
        [SerializeField]
        public LayerMask layer;
        
        [SerializeField]
        public Vector2 rightPoint;
        [SerializeField]
        public float rightDistance;
        [SerializeField]
        public Vector2 leftPoint;
        [SerializeField]
        public float leftDistance;
        [SerializeField]
        public float topOffset;     // 与顶部监测球的偏移量 
        
        [SerializeField]
        public Color color;

        [SerializeField, ReadOnly]
        private bool isTouchWall;

        /// <summary>
        /// 是否触碰到墙壁
        /// </summary>
        public bool IsTouchWall => isTouchWall;

        /// <summary>
        /// 碰触墙壁的方向，0为未碰到墙壁，1为右侧墙壁，-1为左侧墙壁
        /// </summary>
        public int TouchWallDirection { get; private set; }

        /// <summary>
        /// 顶部位置检测到的碰撞点
        /// </summary>
        public Vector2 TouchWallTopPoint => _topTouchPoint;

        /// <summary>
        /// 中间位置检测到的碰撞点
        /// </summary>
        public Vector2 TouchWallMiddlePoint => _middleTouchPoint;
        


        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];
        private Vector2 _topTouchPoint;
        private Vector2 _middleTouchPoint;
        private float _pausedTime;

        public bool Detect()
        {
            // 中间位置检测
            var position = transform.position;
            var direction = DetectByPosition(position, out _middleTouchPoint);
            if (direction == 0)
            {
                TouchWallDirection = 0;
                return false;
            }

            // 顶部位置检测
            position.y += topOffset;
            direction = DetectByPosition(position, out _topTouchPoint);
            if (direction == 0)
            {
                TouchWallDirection = 0;
                return false;
            }

            TouchWallDirection = direction;
            return true;
        }

        public void DrawGizmos()
        {
            Gizmos.color = color;
            var position = transform.position;
            Gizmos.DrawLine(position + (Vector3)rightPoint, position + (Vector3)rightPoint + new Vector3(rightDistance, 0));
            Gizmos.DrawLine(position + (Vector3)leftPoint, position + (Vector3)leftPoint + new Vector3(-leftDistance, 0));
            
            Gizmos.DrawLine(position + (Vector3)rightPoint + new Vector3(0, topOffset), position + (Vector3)rightPoint + new Vector3(rightDistance, topOffset));
            Gizmos.DrawLine(position + (Vector3)leftPoint+ new Vector3(0, topOffset), position + (Vector3)leftPoint + new Vector3(-leftDistance, topOffset));
        }

        public void Pause(float t)
        {
            _pausedTime = t;
        }

        public void Resume()
        {
            _pausedTime = 0;
        }

        private int DetectByPosition(Vector2 pos, out Vector2 touchPoint)
        {
            var count = Physics2D.RaycastNonAlloc(pos + rightPoint, Vector2.right, _hits, rightDistance, layer);
            if (count > 0 && _hits[0].collider.CompareTag(Tags.Wall))
            {
                touchPoint = _hits[0].point;
                return 1;
            }
            count = Physics2D.RaycastNonAlloc(pos + leftPoint, Vector2.left, _hits, leftDistance, layer);
            if (count > 0 && _hits[0].collider.CompareTag(Tags.Wall))
            {
                touchPoint = _hits[0].point;
                return -1;
            }
            touchPoint = Vector2.zero;
            return 0;
        }

        private void FixedUpdate()
        {
            if (_pausedTime > 0)
            {
                isTouchWall = false;
                TouchWallDirection = 0;
                _pausedTime -= Time.fixedDeltaTime;
                return;
            }
            isTouchWall = Detect();
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