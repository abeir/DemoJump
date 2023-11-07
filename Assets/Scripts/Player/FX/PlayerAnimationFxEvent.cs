using Common.Event;

namespace Player.FX
{
    public struct PlayerAnimationFxEvent : IEvent
    {
        private const string dustGroupName = "DustGroup";
        private const string dustGroupFollowName = "DustGroup_follow";

        public string Group { get; }
        public string FX { get; }

        public PlayerAnimationFxEvent(string group, string fx)
        {
            Group = group;
            FX = fx;
        }

        public static void TriggerJumpDust()
        {
            EventManager.TriggerEvent(new PlayerAnimationFxEvent(dustGroupName, "JumpDust"));
        }

        public static void TriggerDoubleJumpDust()
        {
            EventManager.TriggerEvent(new PlayerAnimationFxEvent(dustGroupName, "DoubleJumpDust"));
        }

        public static void TriggerDashDust()
        {
            EventManager.TriggerEvent(new PlayerAnimationFxEvent(dustGroupName, "DashDust"));
        }

        public static void TriggerLandDust()
        {
            EventManager.TriggerEvent(new PlayerAnimationFxEvent(dustGroupName, "LandDust"));
        }

        public static void TriggerSlideDust()
        {
            EventManager.TriggerEvent(new PlayerAnimationFxEvent(dustGroupName, "SlideDust"));
        }

        public static void TriggerRunDust()
        {
            EventManager.TriggerEvent(new PlayerAnimationFxEvent(dustGroupName, "RunDust"));
        }

        public static void TriggerRunToIdleDust()
        {
            EventManager.TriggerEvent(new PlayerAnimationFxEvent(dustGroupName, "RunToIdleDust"));
        }
    }
}