using Common.Helper;
using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{

    public class JumpState : AStateBase
    {
        public static readonly int JumpHash = Animator.StringToHash("Jump");

        // 这是一个用于修复跳跃时仍判断在地面的特殊值，跳跃的高度必须大于这个值才会进行后续操作
        private const float minJumpHeight = 0.5f;

        private Vector2 _velocity = Vector2.zero;
        private float _positionY;       // 开始跳跃时的Y轴位置
        
        public override StateDefine State { get; } = new StateDefine
        {
            ID = (int)PlayerStateID.Jump,
            Name = PlayerStateID.Jump.ToString()
        };


        public JumpState(PlayerController ctrl) : base(ctrl)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanJump
                   && (PlayerController.IsOnGround
                       || PlayerController.IsOnSlope
                       || PlayerController.JumpCount < 2
                       || pre.ID == (int)PlayerStateID.CoyoteJump);
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> JumpState.OnEnter  pre:{pre.Name}");

            _positionY = PlayerController.transform.position.y;

            PlayerController.JumpCount = 1;

            PlayerController.Rigidbody.AddForce(new Vector2(PlayerController.MoveDirection.x, PlayerController.jumpForce), ForceMode2D.Impulse);

            PlayerController.UnarmedAnimator.SetBool(JumpHash, true);

            if (PlayerController.IsOnGround || PlayerController.IsOnSlope || pre.ID == (int)PlayerStateID.CoyoteJump)
            {
                PlayerFxEvent.TriggerJumpDust();
            }

            // 修改摩擦力
            PlayerController.SetZeroFriction();
        }

        public override void OnExit(StateDefine next)
        {
            _fallSpeed = 0;
            PlayerController.UnarmedAnimator.SetBool(JumpHash, false);
            // 还原摩擦力
            PlayerController.SetDefaultFriction();
        }

        public override void OnStay()
        {
            // [HACK] 此处修复跳远时仍然判断在地面上的问题，计算出跳跃的高度，小于某个高度时不做任何处理
            if (PlayerController.transform.position.y - _positionY < minJumpHeight)
            {
                return;
            }

            if (PlayerController.IsOnAir)
            {
                if (PlayerController.IsTouchLedge)
                {
                    StateMachine.Translate((int)PlayerStateID.LedgeHang);
                }
                else if (PlayerController.IsVelocityYDown)
                {   // 当速度向下，或者松开跳跃键时进入 Fall 状态
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
                else if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }
                else if (PlayerController.JumpPressedThisFrame && PlayerController.CanDoubleJump)
                {
                    StateMachine.Translate((int)PlayerStateID.DoubleJump);
                }
                else if (PlayerController.ClimbPressedKeep)
                {
                    StateMachine.Translate((int)PlayerStateID.WallIdle);
                }
            }
            else
            {
                if (PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
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
                var deceleration = PlayerController.JumpPressedKeep
                    ? PlayerController.jumpDeceleration
                    : PlayerController.jumpReleaseDeceleration;
                _fallSpeed += Time.fixedDeltaTime * deceleration;
                _velocity.y -= _fallSpeed;
            }

            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}