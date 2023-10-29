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
            Name = "Run"
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

            PlayerController.UnarmedAnimator.SetBool(MoveHash, true);
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
                if (Mathf.Abs(PlayerController.MoveDirection.x) > 0)
                {
                    StateMachine.Translate((int)PlayerStateID.CoyoteJump);
                }
            }
            else
            {
                PlayerController.ResetJumpCount();

                if (Mathf.Abs(PlayerController.MoveDirection.x) > 0 && PlayerController.MoveDirection.y < 0 && PlayerController.JumpPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Slide);
                }
                else if (PlayerController.JumpPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Jump);
                }
                else if (Mathf.Abs(PlayerController.MoveDirection.x) < Maths.TinyNum)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
            }
            PlayerController.Flip();
        }
        
        
        public override void OnFixedStay()
        {
            _direction.Set(PlayerController.MoveDirection.x, 0f);
            
            _velocity = Time.fixedDeltaTime * PlayerController.speed * _direction;
            
            PlayerController.Rigidbody.velocity = _velocity;

            PlayerController.UnarmedAnimator.SetFloat(VelocityXHash, Mathf.Abs(PlayerController.MoveDirection.x));
        }
    }
}