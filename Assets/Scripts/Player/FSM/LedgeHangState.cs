using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class LedgeHangState : AStateBase
    {
        public static readonly int LedgeHangHash = Animator.StringToHash("LedgeHang");

        private bool _facingPositive;
        private float _gravityScale;

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.LedgeHang,
            Name = PlayerStateID.LedgeHang.ToString()
        };

        public LedgeHangState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.IsTouchLedge && IsTouchSameWithFacing;
        }

        public override void OnEnter(StateDefine pre)
        {
            PlayerController.Rigidbody.velocity = Vector2.zero;

            _gravityScale = PlayerController.Rigidbody.gravityScale;
            PlayerController.Rigidbody.gravityScale = 0;

            _facingPositive = PlayerController.facingPositive;

            PlayerController.UnarmedAnimator.SetBool(LedgeHangHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.Rigidbody.gravityScale = _gravityScale;

            PlayerController.UnarmedAnimator.SetBool(LedgeHangHash, false);
        }

        public override void OnStay()
        {
            if (IsMoveSameWithFacing)
            {
                StateMachine.Translate((int)PlayerStateID.LedgeClimb);
            }
            else
            {
                // TODO 切换到墙壁下滑状态
            }
        }

        public override void OnFixedStay()
        {
        }

        // 判断面朝向与按键方向是否同向
        private bool IsMoveSameWithFacing
        {
            get
            {
                var moveX = PlayerController.MoveDirection.x;
                return (_facingPositive && moveX > 0) ||
                       (!_facingPositive && moveX < 0);
            }
        }

        // 判断面朝向与触碰边缘的方向是否一致
        private bool IsTouchSameWithFacing =>
            (PlayerController.facingPositive && PlayerController.TouchLedgeDirection == 1) ||
            (!PlayerController.facingPositive && PlayerController.TouchLedgeDirection == -1);
    }
}