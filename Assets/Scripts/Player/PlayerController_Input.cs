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


        private InputType _lockedInputType = InputType.None;

        public void LockInputAll()
        {
            _lockedInputType = InputType.Move | InputType.Jump | InputType.Dash | InputType.Attack;
        }

        public void UnlockInputAll()
        {
            _lockedInputType = InputType.None;
        }
        
        public void LockInput(InputType inputType)
        {
            _lockedInputType |= inputType;
        }

        public void UnlockInput(InputType inputType)
        {
            _lockedInputType &= ~inputType;
        }

        public bool IsLockInput(InputType inputType)
        {
            return (_lockedInputType & inputType) > 0;
        }
        
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (IsLockInput(InputType.Move))
            {
                return;
            }
            
            if (context.performed)
            {
                MoveDirection = context.ReadValue<Vector2>();
                _stateMachine.Translate((int)PlayerStateID.Run);
            }
            else if (context.canceled)
            {
                MoveDirection = context.ReadValue<Vector2>();    
                _stateMachine.Translate((int)PlayerStateID.Idle);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (IsLockInput(InputType.Jump))
            {
                return;
            }
            if (context.performed)
            {
                _stateMachine.Translate((int)PlayerStateID.Jump);
            }
            else if (context.canceled)
            {
                _stateMachine.Translate((int)PlayerStateID.Fall);    
            }
        }


        public void OnDash(InputAction.CallbackContext context)
        {
            if (IsLockInput(InputType.Dash))
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