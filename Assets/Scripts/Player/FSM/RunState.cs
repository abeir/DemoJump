using Common.Helper;
using FSM;
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
            return PlayerController.IsOnGround || PlayerController.IsOnSlope;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> RunState.OnEnter  pre:{pre.Name}");

            PlayerController.UnarmedAnimator.SetBool(MoveHash, true);

            _velocity = Vector2.zero;
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(MoveHash, false);

            if (Mathf.Abs(PlayerController.MoveDirection.x) > Maths.TinyNum)
            {
                return;
            }
            _velocity.Set(0f, PlayerController.Rigidbody.velocity.y);
            PlayerController.Rigidbody.velocity = _velocity;
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
                if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }
                else if (PlayerController.SlidePressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Slide);
                }
                else if (PlayerController.JumpPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Jump);
                }
                else if (!PlayerController.AxisXPressed && Mathf.Abs(PlayerController.Rigidbody.velocity.x) < Maths.TinyNum)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
            }
            PlayerController.Flip();
        }
        
        
        public override void OnFixedStay()
        {
            _direction.Set(PlayerController.MoveDirection.x, 0f);

            if (PlayerController.AxisXPressed)
            {
                _velocity.x = Mathf.Lerp(_velocity.x, Time.fixedDeltaTime * PlayerController.speed * _direction.x, 0.3f);
            }
            else
            {
                _velocity = Time.fixedDeltaTime * PlayerController.speed * _direction;
            }

            PlayerController.Rigidbody.velocity = _velocity;
            PlayerController.UnarmedAnimator.SetFloat(VelocityXHash, Mathf.Abs(PlayerController.MoveDirection.x));
        }
    }
}