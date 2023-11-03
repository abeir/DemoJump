using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class WallClimbState : AStateBase
    {
        public static readonly int WallClimbHash = Animator.StringToHash("WallClimb");

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.WallClimb,
            Name = PlayerStateID.WallClimb.ToString()
        };

        private float _gravityScale;
        private Vector2 _velocity;

        public WallClimbState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanWall && PlayerController.IsTouchWall;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> WallClimbState.OnEnter  pre:{pre.Name}");
            _velocity = Vector2.zero;

            _gravityScale = PlayerController.Rigidbody.gravityScale;
            PlayerController.Rigidbody.gravityScale = 0;

            PlayerController.UnarmedAnimator.SetBool(WallClimbHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.Rigidbody.velocity = Vector2.zero;
            PlayerController.Rigidbody.gravityScale = _gravityScale;

            PlayerController.UnarmedAnimator.SetBool(WallClimbHash, false);
        }

        public override void OnStay()
        {
            if (PlayerController.ClimbPressedKeep)
            {
                if (PlayerController.JumpPressedThisFrame)
                {
                    StateMachine.Translate((int)PlayerStateID.WallJump);
                }
                else if (PlayerController.DownPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.WallSlide);
                }
                else if (!PlayerController.AxisYPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.WallIdle);
                }
                else if (PlayerController.IsTouchLedge)
                {
                    StateMachine.Translate((int)PlayerStateID.LedgeHang);
                }
            }
            else
            {
                if (PlayerController.IsOnGround || PlayerController.IsOnSlope)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
                else
                {
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
            }
        }

        public override void OnFixedStay()
        {
            if (PlayerController.UpPressed)
            {
                _velocity.Set(0, PlayerController.wallClimbSpeed * Time.fixedDeltaTime);
            }
            else
            {
                _velocity.Set(0, 0);
            }
            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}