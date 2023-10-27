using Common.Helper;
using FSM;
using Player.FSM;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Player
{
    
    public partial class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform unarmedTransform;
        [SerializeField]
        private StateConfig stateConfig;

        [Title("Motion")]
        [SerializeField]
        public bool facingPositive = true;      // 面向正轴
        [SerializeField]
        public float speed = 250;
        [SerializeField]
        public float jumpForce = 20;     // 跳跃的力量
        [SerializeField]
        public float doubleJumpForce = 25;
        [SerializeField]
        public float jumpCacheDuration = 0.1f;      // 跳跃键缓存的时长
        [SerializeField]
        public float coyoteJumpDuration = 0.05f;     // 土狼跳的持续时间
        [SerializeField]
        public float jumpDeceleration = 10; // 跳跃时的减速度
        [SerializeField]
        public float maxFallVelocity = 15;      // 最大下落速度
        [SerializeField, Range(0f, 0.5f)]
        public float fallAcceleration = 0.1f;      // 下落加速度

        [SerializeField]
        public float dashSpeed = 550;       // 冲刺速度
        [SerializeField]
        public float dashCoolingTime = 0.5f;      // 冲刺冷却时间
        [SerializeField]
        public float dashDuration = 0.3f;       // 冲刺持续时间

        [SerializeField]
        public float slideSpeed = 450;      // 滑行速度
        [SerializeField]
        public float slideCoolingTime = 0.5f;      // 滑行冷却时间
        [SerializeField]
        public float slideDuration = 0.3f;       // 滑行持续时间



        [Title("Other")]
        [SerializeField]
        public bool debug = false;      // 开关调试模式
        
        public Animator UnarmedAnimator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        
        public CapsuleCollider2D Collider { get; private set; }
        
        public PlayerDetector PlayerDetector { get; private set; }
        
        public PlayerInputLock InputLock { get; private set; }

        public int JumpCount { get; set; }      // 跳跃次数

        
        #region 内部变量

        private InputActions _inputActions;
        private StateMachine _stateMachine;

        #endregion
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();

            Collider = unarmedTransform.GetComponent<CapsuleCollider2D>();
            UnarmedAnimator = unarmedTransform.GetComponent<Animator>();
            SpriteRenderer = unarmedTransform.GetComponent<SpriteRenderer>();

            PlayerDetector = GetComponentInChildren<PlayerDetector>();
            
            _inputActions = new InputActions();
            _inputActions.Gameplay.SetCallbacks(this);

            InputLock = new PlayerInputLock();
        }

        private void Start()
        {
            _stateMachine = StateHelper.CreateStateMachine(stateConfig, this);
        }

        private void OnEnable()
        {
            EnableInputGameplay(true);
        }

        private void OnDisable()
        {
            EnableInputGameplay(false);
        }

        private void OnDestroy()
        {
            _inputActions.Dispose();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
            
            // 清除跳跃按键状态，防止多次跳跃
            ResetJumpPressed();
        }

        public bool Flip()
        {
            if (!facingPositive && MoveDirection.x >= Maths.TinyNum)
            {
                facingPositive = true;
                SpriteRenderer.flipX = false;
                return true;
            }
            else if (facingPositive && MoveDirection.x <= -Maths.TinyNum)
            {
                facingPositive = false;
                SpriteRenderer.flipX = true;
                return true;
            }
            return false;
        }

        public void ResetJumpCount()
        {
            JumpCount = 0;
        }

        private void ResetJumpPressed()
        {
            if (JumpPressed && Time.time - _jumpCacheTime > jumpCacheDuration)
            {
                JumpPressed = false;
            }
        }
    }
}