using System;

namespace Player.Enums
{
    [Flags]
    public enum JumpMode
    {
        None = 0,
        OnGround = 1,
        OnSlope = 1 << 1,
        WhenFalling = 1 << 2,
        DoubleJump = 1 << 3

    }


    public static class JumpModeExtensions
    {
        public static bool Contains(this JumpMode self, JumpMode mode)
        {
            return (self & mode) > 0;
        }
    }

}