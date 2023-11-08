using Common.Helper;
using Common.Settings;
using FSM;
using Platform;
using UnityEngine;

namespace Player.FSM
{
    public class CrouchState : AStateBase
    {
        public static readonly int CrouchHash = Animator.StringToHash("Crouch");


        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.Crouch,
            Name = PlayerStateID.Crouch.ToString()
        };


        private Vector2 _colliderSize;
        private Vector2 _colliderOffset;
        private Vector2 _velocity;

        public CrouchState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanCrouch
                && (PlayerController.IsOnGround || PlayerController.IsOnSlope);
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> CrouchState.OnEnter  pre:{pre.Name}");
            
            var collider = PlayerController.UnarmedCollider;
            _colliderSize = collider.size;
            _colliderOffset  = collider.offset;

            collider.size = PlayerController.crouchColliderSize;
            collider.offset = PlayerController.crouchColliderOffset;

            _velocity = PlayerController.Rigidbody.velocity;

            PlayerController.UnarmedAnimator.SetBool(CrouchHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedCollider.size = _colliderSize;
            PlayerController.UnarmedCollider.offset = _colliderOffset;

            PlayerController.UnarmedAnimator.SetBool(CrouchHash, false);
        }

        public override void OnStay()
        {
            if (PlayerController.IsOnAir)
            {
                StateMachine.Translate((int)PlayerStateID.Fall);
                return;
            }
            if (PlayerController.CrouchPressed)
            {
                if (PlayerController.JumpPressedThisFrame && (PlayerController.IsOnOneWayPlatform || PlayerController.IsOnMovingPlatform))
                {
                    // TODO 跃下单向平台
                    Debug.Log("TODO 跃下单向平台");

                    SuspendPlatformEvent.TriggerEvent(Layers.Player);
                }
                else if (PlayerController.SlidePressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Slide);
                }
                else if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }
                else if (PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Crawl);
                }
            }
            else
            {
                if (PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
                else if (!PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
            }
        }

        public override void OnFixedStay()
        {
            if (Mathf.Abs(_velocity.x) > Maths.TinyNum)
            {
                _velocity.x = Mathf.Lerp(_velocity.x, 0, PlayerController.crawlDeceleration);
                PlayerController.Rigidbody.velocity = _velocity;
            }
        }
    }
}