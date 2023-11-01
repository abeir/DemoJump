using System;
using System.Collections.Generic;
using System.Linq;
using FSM.State;
using UnityEngine;

namespace FSM
{
    public class SimpleStateMachine : IStateMachine
    {
        private readonly Dictionary<int, IStateBase> _states = new();

        private int _defaultStateID = (int)StateID.None;

        private IStateBase _current;
        private IStateBase _previous;

        private bool _init;
        private bool _debug;


        public StateDefine Current => _current.State;

        public StateDefine Previous => _previous?.State;

        public void SetDebug(bool debug)
        {
            _debug = debug;
        }

        public void Init()
        {
            if (_init)
            {
                return;
            }
            _init = true;

            var entranceState = new EntranceState();
            AddState(entranceState);

            _current = entranceState;
            var selected = SelectDefaultState();

            // 由于调用 Init 的时候，如地面检测等检测行为可能还没有进行，所需要在此处强行进入默认状态
            ChangeState(selected.State.ID);
        }

        public void SetDefaultState(int id)
        {
            if (!_states.ContainsKey(id))
            {
                throw new ArgumentException("Not found state");
            }
            _defaultStateID = id;
        }

        public void AddState(IStateBase state)
        {
            state.StateMachine = this;
            _states[state.State.ID] = state;
        }

        public void AddTranslate(int originID, int targetID)
        {
        }

        public void AddAnyTranslate(int targetID)
        {
        }

        public void Translate(int nextID)
        {
#if UNITY_EDITOR
            if (!_init)
            {
                Debug.Assert(_init, "StateMachine not init");
            }
#endif
            if (nextID == _current.State.ID)
            {
                return;
            }
            var nextState = _states[nextID];
            if (!nextState.CanEnter(_current.State))
            {
                Print($"Cannot translation from current state: {_current.State.ID}[{_current.State.Name}], cannot enter target state: {nextID}[{nextState.State.Name}]");
                return;
            }

            ChangeState(nextID);
        }

        public void Update()
        {
            _current.OnStay();
        }

        public void FixedUpdate()
        {
            _current.OnFixedStay();
        }

        public IStateBase Find(int id)
        {
            return !_states.TryGetValue(id, out var state) ? null : state;
        }


        // 当前状态为空时，需要选择一个状态作为当前状态，若设置了默认状态则使用此状态，否则会选取id最小的状态作为默认状态和当前状态
        private IStateBase SelectDefaultState()
        {
            if (_defaultStateID < 0)
            {
                _defaultStateID = _states.Keys.Min();
            }
            return  _states[_defaultStateID];
        }

        private void ChangeState(int nextID)
        {
            var oldState = _current;
            var nextState = _states[nextID];
            _previous = oldState;
            _current = nextState;
            oldState.OnExit(nextState.State);
            nextState.OnEnter(oldState.State);
        }

        private void Print(string msg, bool warning = false)
        {
            if (!_debug)
            {
                return;
            }

            if (warning)
            {
                Debug.LogWarning(msg);
            }
            else
            {
                Debug.Log(msg);
            }
        }
    }
}