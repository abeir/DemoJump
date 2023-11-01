using Player;

namespace FSM
{
    public interface IStateBase
    {
        
        public StateDefine State { get; }
        
        public IStateMachine StateMachine { set; }
        
        public bool CanEnter(StateDefine pre);

        public void OnEnter(StateDefine pre);
        public void OnExit(StateDefine next);

        public void OnStay();

        public void OnFixedStay();
    }


    public abstract class AStateBase : IStateBase
    {
        public abstract StateDefine State { get; }
        
        public IStateMachine StateMachine { protected get; set; }

        protected PlayerController PlayerController { get; private set; }
        
        protected AStateBase(PlayerController ctrl)
        {
            PlayerController = ctrl;
        }

        public abstract bool CanEnter(StateDefine pre);

        public abstract void OnEnter(StateDefine pre);

        public abstract void OnExit(StateDefine next);

        public abstract void OnStay();
        
        public abstract void OnFixedStay();
    }

    public abstract class AStateBaseAdapter : AStateBase
    {
        
        protected AStateBaseAdapter(PlayerController ctrl) : base(ctrl)
        {
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