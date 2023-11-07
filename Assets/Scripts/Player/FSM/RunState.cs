using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{
    public class RunState : AStateBase
    {
        public static readonly int MoveHash = Animator.StringToHash("Move");
        public static readonly int VelocityXHash = Animator.StringToHash("VelocityX");


        private Vector2 _direction = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;
        private float _inAirStartTime;      // 记录进入空中的开始时间，每次进入Run状态时将其设置为负值，表示为不存在开始时间

        private PlayerParticleFxEvent.RunDust _runDust = new();
        
        public override StateDefine State { get; } = new StateDefine
        {
            ID = (int)PlayerStateID.Run,
            Name = PlayerStateID.Run.ToString()
        };
        
        
        public RunState(PlayerController ctrl) : base(ctrl)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanMove && PlayerController.IsOnGround || PlayerController.IsOnSlope;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> RunState.OnEnter  pre:{pre.Name}");

            PlayerController.UnarmedAnimator.SetBool(MoveHash, true);

            _velocity = Vector2.zero;

            // _runDust.direction = PlayerController.facingPositive ? 1 : -1;
            // _runDust.start = true;
            // _runDust.TriggerEvent();

            PlayerAnimationFxEvent.TriggerRunDust();
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(MoveHash, false);

            // _runDust.direction = PlayerController.facingPositive ? 1 : -1;
            // _runDust.start = false;
            // _runDust.TriggerEvent();
        }

        public override void OnStay()
        {
            if (PlayerController.IsOnAir)
            {
                if (PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.CoyoteJump);
                }
            }
            else
            {
                PlayerController.ResetJumpCount();

                if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }
                else if (PlayerController.SlidePressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Slide);
                }
                else if (PlayerController.JumpPressedThisFrame
                         && (PlayerController.CanJumpOnGround || PlayerController.CanJumpOnSlope))
                {
                    StateMachine.Translate((int)PlayerStateID.Jump);
                }
                else if (PlayerController.DownPressed && PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Crawl);
                }
                else if (PlayerController.DownPressed && !PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Crouch);
                }
                else if (PlayerController.ClimbPressedKeep)
                {
                    StateMachine.Translate((int)PlayerStateID.WallIdle);
                }
                else if (!PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
            }

            if (PlayerController.Flip())
            {
                PlayerAnimationFxEvent.TriggerRunDust();
            }
        }
        
        
        public override void OnFixedStay()
        {
            _direction.Set(PlayerController.MoveDirection.x, 0f);

            if (PlayerController.AxisXPressed)
            {
                _velocity.x = Mathf.Lerp(_velocity.x, Time.fixedDeltaTime * PlayerController.moveSpeed * _direction.x, PlayerController.moveAcceleration);
                PlayerController.Rigidbody.velocity = _velocity;
            }
            PlayerController.UnarmedAnimator.SetFloat(VelocityXHash, Mathf.Abs(_direction.x));
        }
    }
}