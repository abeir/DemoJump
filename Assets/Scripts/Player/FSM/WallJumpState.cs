using Common.Helper;
using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{
    public class WallJumpState : AStateBase
    {
        public static readonly int WallJumpHash = Animator.StringToHash("WallJump");


        private Vector2 _velocity = Vector2.zero;
        private float _fallSpeed;

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.WallJump,
            Name = PlayerStateID.WallJump.ToString()
        };

        public WallJumpState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanWallJump && PlayerController.IsTouchLedge;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> WallJumpState.OnEnter");

            PlayerController.Flip();

            PlayerController.Rigidbody.AddForce(new Vector2(PlayerController.MoveDirection.x, PlayerController.jumpForce), ForceMode2D.Impulse);

            PlayerController.UnarmedAnimator.SetBool(WallJumpHash, true);

            PlayerFxEvent.TriggerJumpDust();

            // 修改摩擦力
            PlayerController.SetZeroFriction();
        }

        public override void OnExit(StateDefine next)
        {
            _fallSpeed = 0;
            PlayerController.UnarmedAnimator.SetBool(WallJumpHash, false);
            // 还原摩擦力
            PlayerController.SetDefaultFriction();
        }

        public override void OnStay()
        {
            if (PlayerController.IsOnAir)
            {
                if (PlayerController.IsTouchLedge)
                {
                    StateMachine.Translate((int)PlayerStateID.LedgeHang);
                }
                else if (PlayerController.IsVelocityYDown || !PlayerController.JumpPressedKeep)
                {   // 当速度向下，或者松开跳跃键时进入 Fall 状态
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
                else if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }
            }
            else
            {
                if (!PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
                else if (PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
            }

            PlayerController.Flip();
        }

        public override void OnFixedStay()
        {
            _velocity.x = Time.fixedDeltaTime * PlayerController.speed * PlayerController.MoveDirection.x;
            _velocity.y = PlayerController.Rigidbody.velocity.y;

            // 跳跃时减速
            if (PlayerController.Rigidbody.velocity.y > Maths.TinyNum)
            {
                _fallSpeed += Time.fixedDeltaTime * PlayerController.jumpDeceleration;
                _velocity.y -= _fallSpeed;
            }

            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}