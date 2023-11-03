using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class CrawlState : AStateBase
    {
        public static readonly int CrawlHash = Animator.StringToHash("Crawl");

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.Crawl,
            Name = PlayerStateID.Crawl.ToString()
        };


        private Vector2 _colliderSize;
        private Vector2 _colliderOffset;
        private Vector2 _direction = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;

        public CrawlState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.IsOnGround;
        }

        public override void OnEnter(StateDefine pre)
        {
            var collider = PlayerController.UnarmedCollider;
            _colliderSize = collider.size;
            _colliderOffset  = collider.offset;

            collider.size = PlayerController.crouchColliderSize;
            collider.offset = PlayerController.crouchColliderOffset;

            PlayerController.UnarmedAnimator.SetBool(CrawlHash, true);

            _velocity = Vector2.zero;
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedCollider.size = _colliderSize;
            PlayerController.UnarmedCollider.offset = _colliderOffset;

            PlayerController.UnarmedAnimator.SetBool(CrawlHash, false);
        }

        public override void OnStay()
        {
            if (PlayerController.CrouchPressed)
            {
                if (!PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Crouch);
                }
                else if (PlayerController.JumpPressedImpulse && PlayerController.IsOnOneWayPlatform)
                {
                    // TODO 跃下单向平台
                    Debug.Log("TODO 跃下单向平台");
                }
                else if (PlayerController.SlidePressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Slide);
                }
                else if (PlayerController.DashPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.Dash);
                }

                PlayerController.Flip();
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
            _direction.Set(PlayerController.MoveDirection.x, 0f);

            if (PlayerController.AxisXPressed)
            {
                _velocity.x = Mathf.Lerp(_velocity.x, Time.fixedDeltaTime * PlayerController.crawlSpeed * _direction.x, 0.3f);
            }
            else
            {
                _velocity = Time.fixedDeltaTime * PlayerController.crawlSpeed * _direction;
            }

            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}