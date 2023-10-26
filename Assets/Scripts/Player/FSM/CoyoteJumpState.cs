using Common.Helper;
using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class CoyoteJumpState : AStateBase
    {
        private Vector2 _direction = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;
        private float _startTime;
        private float _gravityScale;

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.CoyoteJump,
            Name = "CoyoteJump"
        };

        public CoyoteJumpState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.PlayerDetector.IsOnAir;
        }

        public override void OnEnter(StateDefine pre)
        {
            _startTime = Time.time;
            _gravityScale = PlayerController.Rigidbody.gravityScale;
            PlayerController.Rigidbody.gravityScale = 0;
            PlayerController.UnarmedAnimator.SetBool(RunState.MoveHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.Rigidbody.gravityScale = _gravityScale;
            PlayerController.UnarmedAnimator.SetBool(RunState.MoveHash, false);

            if (Mathf.Abs(PlayerController.MoveDirection.x) > Maths.TinyNum)
            {
                return;
            }
            _velocity.Set(0f, PlayerController.Rigidbody.velocity.y);
            PlayerController.Rigidbody.velocity = _velocity;
        }

        public override void OnStay()
        {
            if (PlayerController.PlayerDetector.IsOnAir)
            {
                if (Time.time - _startTime > PlayerController.coyoteJumpDuration)
                {
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
            }
            else
            {
                PlayerController.ResetJumpCount();

                if (PlayerController.JumpPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Jump);
                }
                else if (Mathf.Abs(PlayerController.MoveDirection.x) < Maths.TinyNum)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
                else if (Mathf.Abs(PlayerController.MoveDirection.x) > 0)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
            }
            PlayerController.Flip();
        }

        public override void OnFixedStay()
        {
            _direction.Set(PlayerController.MoveDirection.x, 0f);

            _velocity = Time.fixedDeltaTime * PlayerController.speed * _direction;

            PlayerController.Rigidbody.velocity = _velocity;

            PlayerController.UnarmedAnimator.SetFloat(RunState.VelocityXHash, Mathf.Abs(PlayerController.MoveDirection.x));
        }
    }
}