using Common.Helper;
using Player.FSM;
using UnityEngine;


namespace Player
{
    public partial class PlayerController
    {


        public bool CanFall => !PlayerDetector.IsOnGround 
                               && !PlayerDetector.IsHang
                               && Rigidbody.velocity.y < 0f 
                               && (_stateMachine.Current.ID == (int)PlayerStateID.Run ||
                                               _stateMachine.Current.ID == (int)PlayerStateID.Idle);


        /*
         * 地面检查存在一定的高度范围，当角色跳跃开始时此时检测区域可能还没有离开地面，若此时进行重置跳跃次数会导致无法进行二段跳。
         * 由于二段跳必然需要点击两次跳跃键，而跳跃时松开按键会进入 Fall，不松开又可能进入攀登或滑墙状态等其他状态，所以需要判断当前状态不是 Jump
         */
        public bool CanResetJump => (PlayerDetector.IsOnGround || PlayerDetector.IsOnSlope) && _stateMachine.Current.ID != (int)PlayerStateID.Jump;



        private void OnAnimationFinished(string animationName)
        {
            if (animationName == "Dash")        // Dash 结束
            {
                if (PlayerDetector.IsOnAir)
                {
                    _stateMachine.Translate((int)PlayerStateID.Fall);
                }
                else if (Mathf.Abs(MoveDirection.x) > Maths.TinyNum)
                {
                    _stateMachine.Translate((int)PlayerStateID.Run);
                }
                else
                {
                    _stateMachine.Translate((int)PlayerStateID.Idle);
                }
                
            }
        }
    }
}