using System;
using JetBrains.Annotations;

namespace FSM
{
    public interface IStateMachine
    {
        /// <summary>
        /// 获取当前状态定义
        /// </summary>
        public StateDefine Current { get; }
        /// <summary>
        /// 获取上一个状态定义<br/>
        /// 需要注意上一个状态可能为空
        /// </summary>
        [CanBeNull]
        public StateDefine Previous { get; }

        public void SetDebug(bool debug);

        /// <summary>
        /// 在使用状态机前需要进行初始化
        /// </summary>
        public void Init();

        /// <summary>
        /// 设置默认状态，状态机初始化时会进入默认状态，设置的默认状态需要先通过 AddState 添加后才有效
        /// </summary>
        /// <param name="id">默认状态ID</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetDefaultState(int id);

        /// <summary>
        /// 添加状态实现
        /// </summary>
        /// <param name="state">状态实现类</param>
        public void AddState(IStateBase state);

        /// <summary>
        /// 添加状态转换规则，由 originID 状态进入 targetID 状态
        /// </summary>
        /// <param name="originID">原状态ID</param>
        /// <param name="targetID">目标状态ID</param>
        public void AddTranslate(int originID, int targetID);

        /// <summary>
        /// 添加由 Any 状态可进入的目标状态
        /// </summary>
        /// <param name="targetID">目标状态ID</param>
        public void AddAnyTranslate(int targetID);

        /// <summary>
        /// 切换状态，优先根据当前状态切换进入 nextID 状态，若失败，则尝试从 Any 状态进入 nextID 状态
        /// </summary>
        /// <param name="nextID">待切换的状态ID</param>
        public void Translate(int nextID);

        /// <summary>
        /// 每帧调用当前状态的 OnStay
        /// </summary>
        public void Update();

        public void FixedUpdate();

        public IStateBase Find(int id);
    }
}