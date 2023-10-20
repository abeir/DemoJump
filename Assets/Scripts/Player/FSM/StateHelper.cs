using System;
using FSM;

namespace Player.FSM
{
    public static class StateHelper
    {


        public static StateMachine CreateStateMachine(StateConfig config, PlayerController ctrl, bool withInit = true)
        {
            var stateMachine = new StateMachine();

            // 创建状态的实例
            var args = new object[] { ctrl };
            foreach (var stateClass in config.nodes)
            {
                var type = Type.GetType(stateClass);
                if (type == null)
                {
                    continue;
                }
                var instance = Activator.CreateInstance(type, args);
                if (instance is IStateBase state)
                {
                    stateMachine.AddState(state);
                }
            }
            // 添加状态转换
            foreach (var translate in config.GetTranslates())
            {
                stateMachine.AddTranslate((int)translate.source, (int)translate.target);
            }

            foreach (var stateID in config.anyTranslates)
            {
                stateMachine.AddAnyTranslate((int)stateID);
            }
            
            stateMachine.SetDefaultState((int)config.defaultState);
            if (withInit)
            {
                stateMachine.Init();
            }
            return stateMachine;
        }
    }
}