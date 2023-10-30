using Unity.Collections;
using UnityEngine;

namespace Common.Detector
{
    public class HangDetector : MonoBehaviour, IDetector
    {

        [SerializeField]
        public bool debugger = true;
        [SerializeField]
        public LayerMask layer;

        [SerializeField]
        public Vector2 rightPoint;
        [SerializeField]
        public Vector2 leftPoint;
        [SerializeField]
        public float distance;
        [SerializeField]
        public float spacing;

        [SerializeField]
        public Color color;

        [SerializeField, ReadOnly]
        private bool isTouchEdge;

        /// <summary>
        /// 是否触碰到边缘
        /// </summary>
        public bool IsTouchEdge => isTouchEdge;
        /// <summary>
        /// 触碰边缘的方向，0表示为碰到边缘，1为右侧碰到，-1为左侧碰到
        /// </summary>
        public int TouchEdgeDirection { get; private set; }
        /// <summary>
        /// 触碰边缘的垂直方向的点
        /// </summary>
        public Vector2 TouchVerticalPoint { get; private set; }
        /// <summary>
        /// 触碰边缘的水平方向的点
        /// </summary>
        public Vector2 TouchHorizontalPoint { get; private set; }


        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];

        public bool Detect()
        {
            var pos = (Vector2)transform.position;
            var isTouched = DetectWithDirection(pos + rightPoint, true);
            if (isTouched)
            {
                TouchEdgeDirection = 1;
                return true;
            }
            isTouched = DetectWithDirection(pos + leftPoint, false);
            if (isTouched)
            {
                TouchEdgeDirection = -1;
                return true;
            }
            TouchEdgeDirection = 0;
            return false;
        }

        public void DrawGizmos()
        {
            Gizmos.color = color;
            var position = transform.position;
            var rightOrigin = position + (Vector3)rightPoint;
            Gizmos.DrawLine(rightOrigin, rightOrigin + new Vector3(distance, 0, 0));
            Gizmos.DrawLine(rightOrigin + new Vector3(0, spacing, 0), rightOrigin + new Vector3(distance, spacing, 0));
            Gizmos.DrawLine(rightOrigin + new Vector3(distance, 0, 0), rightOrigin + new Vector3(distance, spacing, 0));

            var leftOrigin = position + (Vector3)leftPoint;
            Gizmos.DrawLine(leftOrigin, leftOrigin + new Vector3(-distance, 0, 0));
            Gizmos.DrawLine(leftOrigin + new Vector3(0, spacing, 0), leftOrigin + new Vector3(-distance, spacing, 0));
            Gizmos.DrawLine(leftOrigin + new Vector3(-distance, 0, 0), leftOrigin + new Vector3(-distance, spacing, 0));
        }


        private bool DetectWithDirection(Vector2 origin, bool right)
        {
            var direction = right ? Vector2.right : Vector2.left;
            var distanceX = right ? distance : -distance;
            var count = Physics2D.RaycastNonAlloc(origin, direction, _hits, distance, layer);
            if (count < 1)
            {
                TouchVerticalPoint = Vector2.zero;
                TouchHorizontalPoint = Vector2.zero;
                return false;
            }
            var touchVerticalPoint = _hits[0].point;

            count = Physics2D.RaycastNonAlloc(origin + new Vector2(distanceX, 0), Vector2.up, _hits, spacing, layer);
            if (count < 1)
            {
                TouchVerticalPoint = Vector2.zero;
                TouchHorizontalPoint = Vector2.zero;
                return false;
            }
            var touchHorizontalPoint = _hits[0].point;

            count = Physics2D.RaycastNonAlloc(origin + new Vector2(0, spacing), direction, _hits, distanceX, layer);
            if (count < 1)
            {
                TouchVerticalPoint = touchVerticalPoint;
                TouchHorizontalPoint = touchHorizontalPoint;
                return true;
            }
            TouchVerticalPoint = Vector2.zero;
            TouchHorizontalPoint = Vector2.zero;
            return false;
        }

        private void Update()
        {
            isTouchEdge = Detect();
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