using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{
    public class DashState : AStateBase
    {
        // 动画参数
        public static readonly int DashHash = Animator.StringToHash("Dash");


        private float _lastTime;        // 最近一次冲刺时间
        private Vector2 _velocity = Vector2.zero;
        private int _direction;  // 冲刺方向，正为右负为左

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.Dash,
            Name = PlayerStateID.Dash.ToString()
        };
        
        public DashState(PlayerController ctrl) : base(ctrl)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanDash
                && (PlayerController.dashCoolingTime < Time.time - _lastTime || _lastTime == 0);
        }

        public override void OnEnter(StateDefine pre)
        {
            _lastTime = Time.time;
            _velocity = Vector2.zero;

            // 确定冲刺方向
            if (Mathf.Abs(PlayerController.MoveDirection.x) > 0)
            {
                _direction = PlayerController.MoveDirection.x > 0 ? 1 : -1;
            }
            else
            {
                _direction = PlayerController.facingPositive ? 1 : -1;
            }

            PlayerController.Flip(_direction);

            PlayerController.UnarmedAnimator.SetBool(DashHash, true);

            if (PlayerController.IsOnGround || PlayerController.IsOnSlope)
            {
                PlayerFxEvent.TriggerDashDust();
            }
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(DashHash, false);
            
            // 修复退出冲刺后仍然会横向移动问题
            _velocity.Set(0, PlayerController.Rigidbody.velocity.y);
            PlayerController.Rigidbody.velocity = _velocity;
        }

        public override void OnStay()
        {
            if (PlayerController.dashDuration < Time.time - _lastTime)
            {
                // Dash 结束
                if (PlayerController.IsOnAir)
                {
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
                else if (PlayerController.AxisXPressed)
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
            _velocity.x = Time.fixedDeltaTime * PlayerController.dashSpeed * _direction;
            
            PlayerController.Rigidbody.velocity = _velocity;
        }

    }
}