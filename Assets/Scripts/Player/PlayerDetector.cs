using System.Collections;
using Common.Detector;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Player
{
    public class PlayerDetector : ADetectorMonoBehaviour
    {

        [SerializeField, Range(0f, 1f)]
        private float hangPauseDuration = 0.2f;     // 暂停攀爬监测时长

        [Title("Debugging")] 
        [ShowInInspector, ReadOnly]
        public bool IsOnGround { get; private set; }
        [ShowInInspector, ReadOnly] 
        public bool IsOnSlope { get; private set; }
        [ShowInInspector, ReadOnly] 
        public bool IsOnAir => !IsOnGround && !IsOnSlope;
        [ShowInInspector, ReadOnly] 
        public bool IsHang { get; private set; }
        [ShowInInspector, ReadOnly]
        public Vector2 HangLowestPoint { get; private set; } // 悬挂检测的最低点，需要注意，使用此属性的前提必须先判断IsHang为true，因为此值会被缓存，未悬挂时会获取到上一次的值
        [ShowInInspector, ReadOnly]
        public Vector2 HangGroundPoint { get; private set; }    // 悬挂检测的地面表面的点，注意项同 HangLowestPoint


        private GroundDetector _groundDetector;


        private bool _hangDetectPaused;     // 暂停监测攀爬
        private Coroutine _hangDetectPauseCoroutine;

        /// <summary>
        /// 暂停攀爬监测，根据 hangPauseDuration 决定重新启动监测时间
        /// </summary>
        /// <param name="flush">是否刷新暂停时间</param>
        public void HangDetectPause(bool flush = false)
        {
            if (_hangDetectPaused && !flush)
            {
                return;
            }

            if (_hangDetectPauseCoroutine != null)
            {
                StopCoroutine(_hangDetectPauseCoroutine);
                _hangDetectPauseCoroutine = null;
            }
            _hangDetectPauseCoroutine = StartCoroutine(HangDetectPauseCoroutine(hangPauseDuration));
        }

        private IEnumerator HangDetectPauseCoroutine(float duration)
        {
            _hangDetectPaused = true;
            IsHang = false;
            yield return new WaitForSeconds(duration);
            _hangDetectPaused = false;
            _hangDetectPauseCoroutine = null;
        }
        
        private void Awake()
        {
            _groundDetector = new GroundDetector()
            {
                SetDetectorMonoBehaviour = this,
            };

        }

        private void Start()
        {
            _groundDetector.Init();
            
        }


        private void FixedUpdate()
        {
            IsOnGround = _groundDetector.Detect();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (groundDebugger)
            {
                _groundDetector?.DrawGizmos();
            }

        }
#endif
    }

}