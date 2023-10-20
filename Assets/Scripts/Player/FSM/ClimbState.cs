using FSM;

namespace Player.FSM
{
    public class ClimbState : AStateBase
    {
        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.Climb,
            Name = "Climb"
        };

        public ClimbState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return true;
        }

        public override void OnEnter(StateDefine pre)
        {
        }

        public override void OnExit(StateDefine next)
        {
        }

        public override void OnStay()
        {
        }

        public override void OnFixedStay()
        {
        }
    }
}