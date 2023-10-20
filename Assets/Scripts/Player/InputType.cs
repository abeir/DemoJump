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
}