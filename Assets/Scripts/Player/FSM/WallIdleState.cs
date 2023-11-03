using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class WallIdleState : AStateBase
    {
        public static readonly int WallIdleHash = Animator.StringToHash("WallIdle");

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.WallIdle,
            Name = PlayerStateID.WallIdle.ToString()
        };

        private float _gravityScale;

        public WallIdleState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanWall && PlayerController.IsTouchWall;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> WallIdleState.OnEnter  pre:{pre.Name}");

            _gravityScale = PlayerController.Rigidbody.gravityScale;
            PlayerController.Rigidbody.gravityScale = 0;

            PlayerController.UnarmedAnimator.SetBool(WallIdleHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.Rigidbody.gravityScale = _gravityScale;

            PlayerController.UnarmedAnimator.SetBool(WallIdleHash, false);
        }

        public override void OnStay()
        {

            if (PlayerController.ClimbPressedKeep)
            {
                if (PlayerController.JumpPressedThisFrame)
                {
                    StateMachine.Translate((int)PlayerStateID.WallJump);
                }
                else if (PlayerController.UpPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.WallClimb);
                }
                else if (PlayerController.DownPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.WallSlide);
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
        }
    }
}