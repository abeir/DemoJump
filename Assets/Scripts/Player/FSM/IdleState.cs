using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class IdleState : AStateBase
    {
        public static readonly int IdleHash = Animator.StringToHash("Idle");
        
        public override StateDefine State { get; } = new StateDefine()
        {
            ID = (int)PlayerStateID.Idle,
            Name = "Idle"
        };
        
        public IdleState(PlayerController p) : base(p)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            /*
             * [HACK]
             * 此处修复因快速按以下按键导致的跳跃时变为了Idle状态
             * 按下移动 -> 按住跳跃 -> 松开移动
             */
            if (pre.ID == (int)PlayerStateID.Jump)
            {
                return PlayerController.PlayerDetector.IsOnGround && Mathf.Abs(PlayerController.Rigidbody.velocity.y) <= Mathf.Epsilon;
            }
            return PlayerController.PlayerDetector.IsOnGround;
        }

        public override void OnEnter(StateDefine pre)
        {
            PlayerController.UnarmedAnimator.SetBool(IdleHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(IdleHash, false);
        }

        public override void OnStay()
        {
            if (PlayerController.PlayerDetector.IsOnAir)
            {
                if (PlayerController.IsVelocityYDown)
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
                else if (Mathf.Abs(PlayerController.MoveDirection.x) > 0)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
            }
        }

        public override void OnFixedStay()
        {
        }

    }
}