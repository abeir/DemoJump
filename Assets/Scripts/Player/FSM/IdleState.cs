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
            Name = PlayerStateID.Idle.ToString()
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
                return !PlayerController.IsOnAir && Mathf.Abs(PlayerController.Rigidbody.velocity.y) <= Mathf.Epsilon;
            }
            return !PlayerController.IsOnAir;
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
            if (PlayerController.IsOnAir)
            {
                if (PlayerController.IsVelocityYDown)
                {
                    StateMachine.Translate((int)PlayerStateID.Fall);
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
                else if (PlayerController.AxisXPressed)
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