using System;
using System.Collections.Generic;
using System.Linq;
using FSM.State;
using JetBrains.Annotations;
using UnityEngine;
using AnyState = FSM.State.AnyState;

namespace FSM
{
    public sealed class StateMachine
    {
        private readonly Dictionary<int, IStateBase> _states = new();
        private readonly Dictionary<int, HashSet<int>> _translates = new();

        private int _defaultStateID = (int)StateID.None;
        
        private IStateBase _current;
        private IStateBase _previous;

        private bool _init;
        private bool _debug;

        /// <summary>
        /// 获取当前状态定义
        /// </summary>
        public StateDefine Current => _current.State;
        /// <summary>
        /// 获取上一个状态定义<br/>
        /// 需要注意上一个状态可能为空
        /// </summary>
        [CanBeNull]
        public StateDefine Previous => _previous?.State;
        
        public StateMachine(bool debug = false)
        {
            _debug = debug;
        }

        /// <summary>
        /// 设置默认状态，状态机初始化时会进入默认状态，设置的默认状态需要先通过 AddState 添加后才有效
        /// </summary>
        /// <param name="id">默认状态ID</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetDefaultState(int id)
        {
            if (!_states.ContainsKey(id))
            {
                throw new ArgumentException("Not found state");
            }
            _defaultStateID = id;
        }


        /// <summary>
        /// 添加状态实现
        /// </summary>
        /// <param name="state">状态实现类</param>
        public void AddState(IStateBase state)
        {
            state.StateMachine = this;
            _states[state.State.ID] = state;
        }

        /// <summary>
        /// 添加状态转换规则，由 originID 状态进入 targetID 状态
        /// </summary>
        /// <param name="originID">原状态ID</param>
        /// <param name="targetID">目标状态ID</param>
        public void AddTranslate(int originID, int targetID)
        {
            if (_translates.TryGetValue(originID, out var targetIDs))
            {
                targetIDs.Add(targetID);
            }
            else
            {
                HashSet<int> ids = new() { targetID };
                _translates[originID] = ids;
            }
        }

        /// <summary>
        /// 添加由 Any 状态可进入的目标状态
        /// </summary>
        /// <param name="targetID">目标状态ID</param>
        public void AddAnyTranslate(int targetID)
        {
            AddTranslate(AnyStateDefine.Instance.ID, targetID);
        }


        /// <summary>
        /// 切换状态，优先根据当前状态切换进入 nextID 状态，若失败，则尝试从 Any 状态进入 nextID 状态
        /// </summary>
        /// <param name="nextID">待切换的状态ID</param>
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
            
            if (TranslateFromCurrent(nextID))
            {
                return;
            }
            TranslateFromAny(nextID);
        }

        /// <summary>
        /// 初始化，添加 Entrance 和 Any 状态，并将当前状态设置为默认状态后进入当前状态
        /// </summary>
        public void Init()
        {
            _init = true;

            var entranceState = new EntranceState();
            AddState(entranceState);
            AddState(new AnyState());

            _current = entranceState;
            var selected = SelectDefaultState();
            
            AddTranslate(entranceState.State.ID, selected.State.ID);
            
            // 由于调用 Init 的时候，如地面检测等检测行为可能还没有进行，所需要在此处强行进入默认状态
            ChangeState(selected.State.ID);
        }
        
        /// <summary>
        /// 每帧调用当前状态的 OnStay
        /// </summary>
        public void Update()
        {
            _current.OnStay();
        }

        public void FixedUpdate()
        {
            _current.OnFixedStay();
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

        private bool TranslateFromCurrent(int nextID)
        {
            if (!_translates.TryGetValue(_current.State.ID, out var targetIDs))
            {
                Print($"Cannot translation from current state: {_current.State.ID}[{_current.State.Name}], target is empty", true);
                return false;
            }
            if (!targetIDs.Contains(nextID))
            {
                Print($"Cannot translation from current state: {_current.State.ID}[{_current.State.Name}], no target state: {nextID}", true);
                return false;
            }

            var nextState = _states[nextID];
            if (!nextState.CanEnter(_current.State))
            {
                Print($"Cannot translation from current state: {_current.State.ID}[{_current.State.Name}], cannot enter target state: {nextID}[{nextState.State.Name}]");
                return false;
            }

            ChangeState(nextID);
            return true;
        }

        private bool TranslateFromAny(int nextID)
        {
            if (!_translates.TryGetValue(AnyStateDefine.Instance.ID, out var targetIDs))
            {
                return false;
            }
            if (!targetIDs.Contains(nextID))
            {
                Print($"Cannot translation from any, no target state: {nextID}", true);
                return false;
            }

            var nextState = _states[nextID];
            if (!nextState.CanEnter(_current.State))
            {
                Print($"Cannot translation from any, cannot enter target state: {nextID}[{nextState.State.Name}]");
                return false;
            }

            ChangeState(nextID);
            return true;
        }


        public IStateBase Find(int id)
        {
            return !_states.TryGetValue(id, out var state) ? null : state;
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