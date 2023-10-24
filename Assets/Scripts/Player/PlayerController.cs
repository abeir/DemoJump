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
        public float jumpForce = 850;     // 跳跃的力量
        [SerializeField]
        public float doubleJumpForce = 1000;
        [SerializeField]
        public float jumpDeceleration = 10; // 跳跃时的减速度
        [SerializeField]
        public float maxFallVelocity = 30;      // 最大下落速度
        [SerializeField, Range(0f, 0.5f)]
        public float fallAcceleration = 0.15f;      // 下落加速度

        [SerializeField]
        public float dashSpeed = 400;       // 冲刺速度
        [SerializeField]
        public float dashCoolingTime = 1f;      // 冲刺冷却时间


        [Title("Other")]
        [SerializeField]
        public bool debug = false;      // 开关调试模式
        
        public Animator UnarmedAnimator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        
        public CapsuleCollider2D Collider { get; private set; }
        
        public PlayerDetector PlayerDetector { get; private set; }
        
        public PlayerAnimationEventHandler PlayerAnimationEventHandler { get; private set; }

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
            PlayerAnimationEventHandler = unarmedTransform.GetComponent<PlayerAnimationEventHandler>();

            PlayerDetector = GetComponentInChildren<PlayerDetector>();
            
            _inputActions = new InputActions();
            _inputActions.Gameplay.SetCallbacks(this);
        }

        private void Start()
        {
            _stateMachine = StateHelper.CreateStateMachine(stateConfig, this);
        }

        private void OnEnable()
        {
            PlayerAnimationEventHandler.animationFinishAction += OnAnimationFinished;
            
            _inputActions.Gameplay.Enable();
        }

        private void OnDisable()
        {
            PlayerAnimationEventHandler.animationFinishAction -= OnAnimationFinished;
            
            _inputActions.Gameplay.Disable();
        }

        private void OnDestroy()
        {
            _inputActions.Dispose();
        }

        private void Update()
        {
            if (PlayerDetector.IsHang)
            {
                _stateMachine.Translate((int)PlayerStateID.Hang);
            }
            if (CanFall)
            {
                _stateMachine.Translate((int)PlayerStateID.Fall);
            }
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
            
            if (CanResetJump)       // 重置跳跃次数
            {
                JumpCount = 0;
            }
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

    }
}