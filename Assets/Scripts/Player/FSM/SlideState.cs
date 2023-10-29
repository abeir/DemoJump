using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class SlideState : AStateBase
    {
        public static readonly int SlideHash = Animator.StringToHash("Slide");

        private float _lastTime;
        private Vector2 _velocity;
        private int _direction;

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.Slide,
            Name = "Slide"
        };

        public SlideState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.MoveDirection.y < 0 && (PlayerController.slideCoolingTime < Time.time - _lastTime || _lastTime == 0);
        }

        public override void OnEnter(StateDefine pre)
        {
            _lastTime = Time.time;
            _velocity = Vector2.zero;

            // 确定滑行方向
            if (Mathf.Abs(PlayerController.MoveDirection.x) > 0)
            {
                _direction = PlayerController.MoveDirection.x > 0 ? 1 : -1;
            }
            else
            {
                _direction = PlayerController.facingPositive ? 1 : -1;
            }
            PlayerController.UnarmedAnimator.SetBool(SlideHash, true);

            if (PlayerController.IsOnGround || PlayerController.IsOnSlope)
            {
                PlayerFxEvent.TriggerSlideDust();
            }
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(SlideHash, false);

            // 修复退出滑行后仍然会横向移动问题
            _velocity.Set(0, PlayerController.Rigidbody.velocity.y);
            PlayerController.Rigidbody.velocity = _velocity;
        }

        public override void OnStay()
        {
            if (PlayerController.IsOnAir)
            {
                StateMachine.Translate((int)PlayerStateID.Fall);
                return;
            }
            if (PlayerController.slideDuration < Time.time - _lastTime)
            {
                if (Mathf.Abs(PlayerController.MoveDirection.x) > 0)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
                else
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
            }
        }

        public override void OnFixedStay()
        {
            _velocity.x = Time.fixedDeltaTime * PlayerController.slideSpeed * _direction;

            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}