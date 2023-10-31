using FSM;

namespace Player.FSM
{
    public enum PlayerStateID
    {
        Idle = StateID.User + 1,
        Run,
        Jump,
        Fall,
        Dash,
        CoyoteJump,
        Land,
        Slide,
        LedgeHang,
        LedgeClimb
    }
}