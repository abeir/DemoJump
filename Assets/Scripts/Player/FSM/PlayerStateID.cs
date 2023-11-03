using FSM;

namespace Player.FSM
{
    public enum PlayerStateID
    {
        Idle = StateID.User + 1,
        Run,
        Jump,
        DoubleJump,
        Fall,
        Dash,
        CoyoteJump,
        Land,
        Slide,
        LedgeHang,
        LedgeClimb,
        WallIdle,
        WallClimb,
        WallJump,
        WallSlide,
        Crouch,
        Crawl
    }
}