using Common.Helper;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerController : InputActions.IGameplayActions
    {

        public Vector2 MoveDirection { get; private set; }

        /// <summary>
        /// 是否按下了x轴
        /// </summary>
        public bool AxisXPressed => Mathf.Abs(MoveDirection.x) > Maths.TinyNum;
        /// <summary>
        /// 是否按下了y轴
        /// </summary>
        public bool AxisYPressed => Mathf.Abs(MoveDirection.y) > Maths.TinyNum;
        /// <summary>
        /// 是否按下了向上键
        /// </summary>
        public bool UpPressed => MoveDirection.y > 0;
        /// <summary>
        /// 是否按下了向下键
        /// </summary>
        public bool DownPressed => MoveDirection.y < 0;

        /// <summary>
        /// 跳跃按键按下时为true，松开时为false
        /// </summary>
        public bool JumpPressed { get; private set; }

        /// <summary>
        /// 跳跃按键的瞬态，当按下跳跃键后为true，经过一段短暂的时间后为false
        /// </summary>
        public bool JumpPressedImpulse { get; private set; }

        public bool DashPressedImpulse { get; private set; }
        /// <summary>
        /// 是否按下下滑组合按键
        /// </summary>
        public bool SlidePressed => AxisXPressed && DownPressed && JumpPressedImpulse;

        public bool ClimbPressed { get; private set; }


        private bool _inputGameplayEnable;
        private float _jumpCacheTime;   // 由于 OnJump 会使 JumpPressed 长时间为true，需要定时重置
        private float _dashCacheTime;   // 由于 OnDash 会是 DashPressed 长时间为true，需要定时重置


        public void EnableInputGameplay(bool enable)
        {
            // 优化，防止重复调用
            if (_inputGameplayEnable == enable)
            {
                return;
            }
            _inputGameplayEnable = enable;
            if (enable)
            {
                _inputActions.Gameplay.Enable();
            }
            else
            {
                _inputActions.Gameplay.Disable();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (InputLock.IsLocked(InputType.Move))
            {
                return;
            }
            
            if (context.performed)
            {
                MoveDirection = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                MoveDirection = context.ReadValue<Vector2>();    
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (InputLock.IsLocked(InputType.Jump))
            {
                return;
            }
            if (context.performed)
            {
                JumpPressed = true;
                JumpPressedImpulse = true;
                _jumpCacheTime = Time.time;
            }
            else if (context.canceled)
            {
                JumpPressed = false;
                JumpPressedImpulse = false;
            }
        }


        public void OnDash(InputAction.CallbackContext context)
        {
            if (InputLock.IsLocked(InputType.Dash))
            {
                return;
            }
            if (context.performed)
            {
                DashPressedImpulse = true;
                _dashCacheTime = Time.time;
            }
            else if (context.canceled)
            {
                DashPressedImpulse = false;
            }
        }
        
        public void OnClimb(InputAction.CallbackContext context)
        {
            if (InputLock.IsLocked(InputType.Climb))
            {
                return;
            }
            if (context.performed)
            {
                ClimbPressed = true;
            }
            else if (context.canceled)
            {
                ClimbPressed = false;
            }
        }
    }
}