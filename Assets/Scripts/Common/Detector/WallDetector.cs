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
        public Vector2 rightCircleCenter;      // 圆心
        [SerializeField]
        public float rightCircleRadius;        // 半径
        [SerializeField]
        public Vector2 leftCircleCenter;
        [SerializeField]
        public float leftCircleRadius;
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


        private readonly Collider2D[] _colliders = new Collider2D[1];

        public bool Detect()
        {
            var count = Physics2D.OverlapCircleNonAlloc((Vector2)transform.position + rightCircleCenter, rightCircleRadius, _colliders, layer);
            if (count > 0)
            {
                TouchWallDirection = 1;
                return true;
            }
            count = Physics2D.OverlapCircleNonAlloc((Vector2)transform.position + leftCircleCenter, leftCircleRadius, _colliders, layer);
            if (count > 0)
            {
                TouchWallDirection = -1;
                return true;
            }
            TouchWallDirection = 0;
            return false;
        }

        public void DrawGizmos()
        {
            Gizmos.color = color;
            var position = transform.position;
            Gizmos.DrawWireSphere(position + (Vector3)rightCircleCenter, rightCircleRadius);
            Gizmos.DrawWireSphere(position + (Vector3)leftCircleCenter, leftCircleRadius);
        }


        private void FixedUpdate()
        {
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