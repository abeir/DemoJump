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
        Hang,
        Climb,
        CoyoteJump,
        Land,
        Slide
    }
}