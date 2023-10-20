using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class DashState : AStateBase
    {
        public static readonly int DashHash = Animator.StringToHash("Dash");

        private float _lastTime;        // 最近一次冲刺时间
        private Vector2 _velocity = Vector2.zero;
        
        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.Dash,
            Name = "Dash"
        };
        
        public DashState(PlayerController ctrl) : base(ctrl)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.dashCoolingTime < Time.time - _lastTime || _lastTime == 0;
        }

        public override void OnEnter(StateDefine pre)
        {
            _lastTime = Time.time;
            _velocity = Vector2.zero;
            
            PlayerController.UnarmedAnimator.SetBool(DashHash, true);
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
        }

        public override void OnFixedStay()
        {
            _velocity.x = Time.fixedDeltaTime * PlayerController.dashSpeed * (PlayerController.facingPositive ? 1 : -1);
            
            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}