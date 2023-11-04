using System;
using FSM;
using Player.Enums;
using Player.FSM;
using Sirenix.OdinInspector;
using UnityEngine;
using Motion = Player.Enums.Motion;


namespace Player
{
    
    public partial class PlayerController : MonoBehaviour
    {



        [SerializeField]
        private Transform unarmedTransform;
        [SerializeField]
        private GameObject detectorGameObject;
        [SerializeField]
        private StateConfig stateConfig;
        [SerializeField]
        private bool simpleStateMachine;

        [Title("Motion")]
        [SerializeField]
        public bool facingPositive = true;      // 面向正轴
        [SerializeField]
        public Motion motion;

        [FoldoutGroup("Move"), SerializeField]
        public float moveSpeed = 250;
        [FoldoutGroup("Move"), SerializeField, Range(0f, 1f)]
        public float moveAcceleration = 0.3f;       // 移动的加速度
        [FoldoutGroup("Move"), SerializeField, Range(0f, 1f)]
        public float moveDeceleration = 0.4f;       // 移动转换成IDLE时的减速度

        [FoldoutGroup("Jump"), SerializeField]
        public JumpMode jumpMode;      // 跳跃模式
        [FoldoutGroup("Jump"), SerializeField]
        public float jumpForce = 20;     // 跳跃的力量
        [FoldoutGroup("Jump"), SerializeField]
        public float doubleJumpForce = 25;
        [FoldoutGroup("Jump"), SerializeField]
        public float jumpCacheDuration = 0.1f;      // 跳跃键缓存的时长
        [FoldoutGroup("Jump"), SerializeField]
        public float coyoteJumpDuration = 0.05f;     // 土狼跳的持续时间
        [FoldoutGroup("Jump"), SerializeField]
        public float jumpDeceleration = 10; // 跳跃时按住跳跃键的减速度
        [FoldoutGroup("Jump"), SerializeField]
        public float jumpReleaseDeceleration = 100;      // 跳跃时松开跳跃键的减速度
        [FoldoutGroup("Jump"), SerializeField]
        public float maxFallVelocity = 15;      // 最大下落速度
        [FoldoutGroup("Jump"), SerializeField, Range(0f, 0.5f)]
        public float fallAcceleration = 0.1f;      // 下落加速度

        [FoldoutGroup("Dash"), SerializeField]
        public float dashSpeed = 550;       // 冲刺速度
        [FoldoutGroup("Dash"), SerializeField]
        public float dashCoolingTime = 0.5f;      // 冲刺冷却时间
        [FoldoutGroup("Dash"), SerializeField]
        public float dashDuration = 0.3f;       // 冲刺持续时间
        [FoldoutGroup("Dash"), SerializeField]
        public float dashCacheDuration = 0.1f;      // 冲刺缓存的时长

        [FoldoutGroup("Slide"), SerializeField]
        public float slideSpeed = 450;      // 滑行速度
        [FoldoutGroup("Slide"), SerializeField]
        public float slideCoolingTime = 0.5f;      // 滑行冷却时间
        [FoldoutGroup("Slide"), SerializeField]
        public float slideDuration = 0.3f;       // 滑行持续时间

        [FoldoutGroup("Ledge"), SerializeField]
        public float ledgeClimbDuration = 0.6f;     // 边缘爬升的持续时间
        
        [FoldoutGroup("Crouch"), SerializeField]
        public float crawlSpeed = 100;            //蹲下时的移动速度
        [FoldoutGroup("Crouch"), SerializeField, Range(0f, 1f)]
        public float crawlAcceleration = 0.3f;       // 蹲下移动的加速度
        [FoldoutGroup("Crouch"), SerializeField, Range(0f, 1f)]
        public float crawlDeceleration = 0.4f;      // 蹲下移动转换为蹲下静止时的减速度
        [FoldoutGroup("Crouch"), SerializeField]
        public Vector2 crouchColliderOffset;         // 蹲下时的碰撞体偏移量
        [FoldoutGroup("Crouch"), SerializeField]
        public Vector2 crouchColliderSize;         // 蹲下时的碰撞体大小

        [FoldoutGroup("Wall"), SerializeField]
        public float wallClimbSpeed = 100;          // 爬墙速度
        [FoldoutGroup("Wall"), SerializeField]
        public float wallSlideSpeed = 60;       // 在墙上的下滑速度


        [Title("Physic Material")]
        [SerializeField]
        public PhysicsMaterial2D defaultFriction;
        [SerializeField]
        public PhysicsMaterial2D zeroFriction;


        [Title("Other")]
        [SerializeField]
        public bool debug;      // 开关调试模式
        
        public Animator UnarmedAnimator { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        
        public CapsuleCollider2D UnarmedCollider { get; private set; }
        
        public PlayerInputLock InputLock { get; private set; }

        public int JumpCount { get; set; }      // 跳跃次数

        
        #region 内部变量

        private InputActions _inputActions;
        private IStateMachine _stateMachine;

        #endregion
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();

            UnarmedCollider = unarmedTransform.GetComponent<CapsuleCollider2D>();
            UnarmedAnimator = unarmedTransform.GetComponent<Animator>();
            SpriteRenderer = unarmedTransform.GetComponent<SpriteRenderer>();

            FindDetectorComponents();

            _inputActions = new InputActions();
            _inputActions.Gameplay.SetCallbacks(this);

            InputLock = new PlayerInputLock();
        }

        private void Start()
        {
            _stateMachine = simpleStateMachine ? StateHelper.CreateSimpleStateMachine(stateConfig, this) : StateHelper.CreateDefaultStateMachine(stateConfig, this);
            _stateMachine.SetDebug(debug);


            // 默认摩擦力
            Rigidbody.sharedMaterial = defaultFriction;
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
            ResetJumpPressedImpulse();
            // 清除冲刺按键状态，防止连续冲刺
            ResetDashPressed();
        }

        
    }
}