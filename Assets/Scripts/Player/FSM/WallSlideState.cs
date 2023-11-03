using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class WallSlideState : AStateBase
    {
        public static readonly int WallSlideHash = Animator.StringToHash("WallSlide");

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.WallSlide,
            Name = PlayerStateID.WallSlide.ToString()
        };

        private float _gravityScale;

        public WallSlideState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanWall && PlayerController.IsTouchWall;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> WallSlideState.OnEnter  pre:{pre.Name}");

            _gravityScale = PlayerController.Rigidbody.gravityScale;
            PlayerController.Rigidbody.gravityScale = 0;

            PlayerController.UnarmedAnimator.SetBool(WallSlideHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.Rigidbody.gravityScale = _gravityScale;

            PlayerController.UnarmedAnimator.SetBool(WallSlideHash, false);
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
            PlayerController.Rigidbody.velocity =
                new Vector2(0, -PlayerController.wallSlideSpeed * Time.fixedDeltaTime);

        }
    }
}