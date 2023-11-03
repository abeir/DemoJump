using Common.Helper;
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
            return PlayerController.CanLedge && PlayerController.IsTouchLedge && IsTouchSameWithFacing;
        }

        public override void OnEnter(StateDefine pre)
        {
            PlayerController.Rigidbody.velocity = Vector2.zero;

            _gravityScale = PlayerController.Rigidbody.gravityScale;
            PlayerController.Rigidbody.gravityScale = 0;

            _facingPositive = PlayerController.facingPositive;

            PlayerController.UnarmedAnimator.SetBool(LedgeHangHash, true);

            StickToEdge();
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.Rigidbody.gravityScale = _gravityScale;

            PlayerController.UnarmedAnimator.SetBool(LedgeHangHash, false);
        }

        public override void OnStay()
        {
            if (PlayerController.AxisXPressed)
            {
                // 移动方向与面朝向同向
                if (IsMoveSameWithFacing)
                {
                    StateMachine.Translate((int)PlayerStateID.LedgeClimb);
                }
                else
                {
                    if (PlayerController.DashPressedImpulse)
                    {
                        StateMachine.Translate((int)PlayerStateID.Dash);
                    }
                    else if (PlayerController.JumpPressedImpulse)
                    {
                        StateMachine.Translate((int)PlayerStateID.WallJump);
                    }
                }
            }
            else
            {
                if (PlayerController.UpPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.LedgeClimb);
                }
                else if (PlayerController.DownPressed && PlayerController.JumpPressedImpulse)   // 从边缘下落
                {
                    // 暂停边缘检测并重置跳跃按键，以解决进入Fall立刻起跳的问题
                    PlayerController.PauseDetectLedge(0.1f);
                    PlayerController.ResetJumpPressedImpulse(true);
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
                else if (PlayerController.JumpPressedImpulse)
                {
                    StateMachine.Translate((int)PlayerStateID.LedgeClimb);
                }
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
                return (_facingPositive && moveX > Maths.TinyNum) ||
                       (!_facingPositive && moveX < Maths.TinyNum);
            }
        }

        // 判断面朝向与触碰边缘的方向是否一致
        private bool IsTouchSameWithFacing =>
            (PlayerController.facingPositive && PlayerController.TouchLedgeDirection == 1) ||
            (!PlayerController.facingPositive && PlayerController.TouchLedgeDirection == -1);


        // 修正人物位置，黏住边缘
        private void StickToEdge()
        {
            var x = PlayerController.TouchLedgeVerticalPoint.x;
            var y = PlayerController.TouchLedgeHorizontalPoint.y;

            var colliderScale = PlayerController.UnarmedCollider.transform.localScale;
            var colliderSize = PlayerController.UnarmedCollider.size;

            PlayerController.transform.position = new Vector3(x - PlayerController.TouchLedgeDirection * colliderSize.x * colliderScale.x / 2,
                y - colliderSize.y * colliderScale.y, 0);
        }
    }
}