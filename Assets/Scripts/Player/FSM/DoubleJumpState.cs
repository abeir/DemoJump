using Common.Helper;
using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{
    public class DoubleJumpState : AStateBase
    {
        public static readonly int DoubleJumpHash = Animator.StringToHash("DoubleJump");

        private Vector2 _velocity = Vector2.zero;

        public override StateDefine State { get; } = new StateDefine
        {
            ID = (int)PlayerStateID.DoubleJump,
            Name = PlayerStateID.DoubleJump.ToString()
        };


        public DoubleJumpState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.IsOnAir && PlayerController.JumpCount < 2;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> DoubleJumpState.OnEnter  pre:{pre.Name}");

            PlayerController.JumpCount = 2;

            PlayerController.Rigidbody.AddForce(new Vector2(PlayerController.MoveDirection.x, PlayerController.doubleJumpForce), ForceMode2D.Impulse);

            PlayerController.UnarmedAnimator.SetBool(DoubleJumpHash, true);

            PlayerFxEvent.TriggerDoubleJumpDust();
            // 修改摩擦力
            PlayerController.SetZeroFriction();
        }

        public override void OnExit(StateDefine next)
        {
            _fallSpeed = 0;
            PlayerController.UnarmedAnimator.SetBool(DoubleJumpHash, false);
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
            PlayerController.Flip();
        }

        private float _fallSpeed;

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