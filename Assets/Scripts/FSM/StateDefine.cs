using Player.FSM;

namespace FSM
{

    /// <summary>
    /// 状态ID，自定义的状态应该使用 User以上的值
    /// </summary>
    public enum StateID
    {
        None = -1,
        Any = 0,
        Entrance = 1,
        User = 10
    }
    

    public class StateDefine
    {
        public int ID;
        public string Name;     // 状态名
    }

    /// <summary>
    /// Any 状态
    /// </summary>
    public sealed class AnyStateDefine : StateDefine
    {
        
        public static AnyStateDefine Instance => _instance ??= new AnyStateDefine();
        
        private static AnyStateDefine _instance;
        
        private AnyStateDefine()
        {
            ID = (int)StateID.Any;
            Name = "Any";
        }
    }

    /// <summary>
    /// Entrance 状态，此为状态机中的第一个状态
    /// </summary>
    public sealed class EntranceStateDefine : StateDefine
    {
        public static EntranceStateDefine Instance => _instance ??= new EntranceStateDefine();
        
        private static EntranceStateDefine _instance;
        
        private EntranceStateDefine()
        {
            ID = (int)StateID.Entrance;
            Name = "Entrance";
        }
    }
}