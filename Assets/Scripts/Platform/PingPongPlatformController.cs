using Common.Helper;
using Common.Settings;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Platform
{
    public class PingPongPlatformController : APlatformController
    {
        [Title("Movement")]
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

        private readonly Vector3[] _points = new Vector3[2];
        private int _nextIndex;

        private Vector3 _lastPosition;      // 前一次移动的位置
        private Vector3 _diffPosition;      // 与前一次移动位置的差值

        private Vector3 Target => _points[_nextIndex % _points.Length];


        private PlayerController _playerController;

        protected override void Awake()
        {
            base.Awake();

            Init();
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            _tweener.Play();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
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

        private void LateUpdate()
        {
            if (_playerController != null && _playerController.IsOnGround)
            {
                _playerController.transform.position += _diffPosition;
            }
        }


        private void OnMoveUpdate()
        {
            var currentPosition = transform.position;
            _diffPosition = currentPosition - _lastPosition;
            _lastPosition = currentPosition;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == Layers.Player)
            {
                _playerController = other.gameObject.GetComponent<PlayerController>();
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.layer == Layers.Player)
            {
                _playerController = null;
            }
        }

        private void Init()
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

            _lastPosition = transform.position;

            _tweener = transform.DOMove(Target, durationPerPoint)
                .OnUpdate(OnMoveUpdate)
                .SetAutoKill(false)
                .SetEase(Ease.InOutQuad);
            _tweener.Pause();
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