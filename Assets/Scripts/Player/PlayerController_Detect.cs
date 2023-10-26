using Player.FSM;


namespace Player
{
    public partial class PlayerController
    {


        public bool IsVelocityYDown => Rigidbody.velocity.y <= 0;

    }
}