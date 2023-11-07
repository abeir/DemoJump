using System;
using Common.Helper;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Platform
{
    public class PingPongPlatformController : MonoBehaviour
    {

        [SerializeField]
        public float durationPerPoint;
        [SerializeField]
        public Vector2 leftPoint;
        [SerializeField]
        public Vector2 rightPoint;
        [SerializeField]
        public bool beginRight;     // 是否启动时向右

        [Title("Debugging")]
        [SerializeField]
        public float radius = 0.3f;
        [SerializeField]
        public Color color = Color.white;



        private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;

        private Vector3[] _points = new Vector3[2];
        private int _nextIndex;

        private Vector3 Target => _points[_nextIndex % _points.Length];

        private void Awake()
        {
            _nextIndex = 0;
            if (beginRight)
            {
                _points[0] = rightPoint;
                _points[1] = leftPoint;
            }
            else
            {
                _points[0] = leftPoint;
                _points[1] = rightPoint;
            }

            _tweener = transform.DOMove(Target, durationPerPoint)
                .SetAutoKill(false).SetEase(Ease.InOutQuad);
            _tweener.Pause();
        }

        private void OnEnable()
        {
            _tweener.Play();
        }

        private void OnDisable()
        {
            _tweener.Pause();
        }

        private void OnDestroy()
        {
            _tweener.Kill();
        }

        private void Update()
        {
            ChangeTarget();

        }

        private void ChangeTarget()
        {
            if ((Target - transform.position).sqrMagnitude > Maths.TinyNum)
            {
                return;
            }

            _nextIndex++;
            _tweener.ChangeValues(transform.position, Target, durationPerPoint);
        }



        private void OnDrawGizmosSelected()
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(leftPoint, radius);
            Gizmos.DrawWireSphere(rightPoint, radius);
        }
    }
}