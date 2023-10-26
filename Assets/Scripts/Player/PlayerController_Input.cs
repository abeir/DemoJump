using Player.FSM;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public partial class PlayerController : InputActions.IGameplayActions
    {

        [ShowInInspector, ReadOnly]
        public Vector2 MoveDirection { get; private set; }
        [ShowInInspector, ReadOnly]
        public bool JumpPressed { get; set; }


        private bool _inputGameplayEnable;
        private float _jumpCacheTime;   // 由于 OnJump 会使 JumpPressed 长时间为true，需要定时重置


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
                _jumpCacheTime = Time.time;
            }
            else if (context.canceled)
            {
                JumpPressed = false;
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
                _stateMachine.Translate((int)PlayerStateID.Dash);
            }
        }
        
        public void OnClimb(InputAction.CallbackContext context)
        {

        }
    }
}