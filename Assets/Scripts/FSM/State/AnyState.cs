namespace FSM.State
{
    public class AnyState : IStateBase
    {
        public StateDefine State => AnyStateDefine.Instance;
        public IStateMachine StateMachine { get; set; }

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