﻿using Common.Helper;
using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{
    public class JumpState : AStateBase
    {
        public static readonly int JumpHash = Animator.StringToHash("Jump");

        private Vector2 _velocity = Vector2.zero;
        
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
            return PlayerController.IsOnGround || PlayerController.JumpCount < 2;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> OnEnter JumpState  pre:{pre.Name}");

            PlayerController.JumpCount = 1;

            PlayerController.Rigidbody.AddForce(new Vector2(PlayerController.MoveDirection.x, PlayerController.jumpForce), ForceMode2D.Impulse);

            PlayerController.UnarmedAnimator.SetBool(JumpHash, true);

            if (PlayerController.IsOnGround || PlayerController.IsOnSlope)
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
            if (PlayerController.IsOnAir)
            {
                if (PlayerController.IsTouchLedge)
                {
                    StateMachine.Translate((int)PlayerStateID.LedgeHang);
                }
                else if (PlayerController.IsVelocityYDown || !PlayerController.JumpPressed)
                {   // 当速度向下，或者松开跳跃键时进入 Fall 状态
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
                else if (PlayerController.JumpPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Jump);
                }
                else if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }
            }
            else
            {
                PlayerController.ResetJumpCount();

                if (PlayerController.JumpPressedImpulse)
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