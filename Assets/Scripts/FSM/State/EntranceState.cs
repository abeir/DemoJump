namespace FSM.State
{
    public class EntranceState : IStateBase
    {
        public StateDefine State => EntranceStateDefine.Instance;
        public StateMachine StateMachine { get; set; }

        public bool CanEnter(StateDefine pre)
        {
            return true;
        }

        public void OnEnter(StateDefine pre)
        {
        }

        public void OnExit(StateDefine next)
        {
        }

        public void OnStay()
        {
        }

        public void OnFixedStay()
        {
        }
    }
}