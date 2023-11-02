using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{
    public class FallState : AStateBase
    {
        
        public static readonly int FallHash = Animator.StringToHash("Fall");
        
        private Vector2 _velocity = Vector2.zero;

        public override StateDefine State { get; } = new StateDefine
        {
            ID = (int)PlayerStateID.Fall,
            Name = PlayerStateID.Fall.ToString()
        };
        
        public FallState(PlayerController ctrl) : base(ctrl)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.IsOnAir || PlayerController.IsTouchLedge;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> FallState.OnEnter  pre:{pre.Name}");

            _velocity = Vector2.zero;
            PlayerController.UnarmedAnimator.SetBool(FallHash, true);
            
            // 修改摩擦力
            PlayerController.SetZeroFriction();
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(FallHash, false);

            if (next.ID == (int)PlayerStateID.Idle)
            {
                PlayerController.Rigidbody.velocity = new Vector2(1f * PlayerController.MoveDirection.x, 0f);
            }
            else
            {
                PlayerController.Rigidbody.velocity = new Vector2(PlayerController.Rigidbody.velocity.x, 0f);
            }

            _fallSpeed = 0;

            if (next.ID == (int)PlayerStateID.Run)
            {
                PlayerFxEvent.TriggerLandDust();
            }
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
                else if (PlayerController.JumpPressedThisFrame)
                {
                    if (PlayerController.JumpCount == 0)
                    {
                        StateMachine.Translate((int)PlayerStateID.Jump);
                    }
                    else if(PlayerController.JumpCount == 1)
                    {
                            StateMachine.Translate((int)PlayerStateID.DoubleJump);
                    }
                }
                else if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }
            }
            else
            {
                if (PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
                else
                {
                    StateMachine.Translate((int)PlayerStateID.Land);
                }
            }

            PlayerController.Flip();
        }

        private float _fallSpeed;
        
        public override void OnFixedStay()
        {
            _velocity.x = Time.fixedDeltaTime * PlayerController.speed * PlayerController.MoveDirection.x;
            
            _fallSpeed = Mathf.Lerp(_fallSpeed, PlayerController.maxFallVelocity, PlayerController.fallAcceleration);
            _velocity.y = -_fallSpeed;
            
            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}