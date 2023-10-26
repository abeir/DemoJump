using System;

namespace Player
{

    [Flags]
    public enum InputType
    {
        None = 0,
        Move = 1,
        Jump = 1 << 1,
        Dash = 1 << 2,
        Attack = 1 << 3
    }


    public class PlayerInputLock
    {
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

        public bool IsLocked(InputType inputType)
        {
            return (_lockedInputType & inputType) > 0;
        }
    }
}