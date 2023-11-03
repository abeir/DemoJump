using System;

namespace Player.Enums
{
    [Flags]
    public enum Motion
    {
        None = 0,
        Move = 1,
        Jump = 1 << 1,
        Dash = 1 << 2,
        Slide = 1 << 3,
        Ledge = 1 << 4,
        Crouch = 1 << 5,
        WallJump = 1 << 6,

        All = int.MaxValue
    }

    public static class MotionExtensions
    {
        public static bool Contains(this Motion self, Motion m)
        {
            return (self & m) > 0;
        }
    }
}