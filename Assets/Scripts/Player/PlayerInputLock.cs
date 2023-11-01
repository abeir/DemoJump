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
        Attack = 1 << 3,
        Climb = 1 << 4
    }


    public class PlayerInputLock
    {
        private InputType _lockedInputType = InputType.None;

        private InputType _lastLocked;      // 锁定全部输入时，记录下之前的锁定值，以便解锁时恢复到之前的锁定值
        private bool _locked;

        public bool LockInputAll()
        {
            if (_locked)
            {
                return false;
            }
            _locked = true;
            _lastLocked = _lockedInputType;
            _lockedInputType = InputType.Move | InputType.Jump | InputType.Dash | InputType.Attack;
            return true;
        }

        public bool UnlockInputAll()
        {
            if (!_locked)
            {
                return false;
            }
            _locked = false;
            _lockedInputType = _lastLocked;
            return true;
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