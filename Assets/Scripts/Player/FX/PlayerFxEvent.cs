using Common.Event;

namespace Player.FX
{
    public class PlayerFxEvent : IEvent
    {
        private const string dustGroupName = "DustGroup";
        private const string dustGroupFollowName = "DustGroup_follow";

        public string Group { get; }
        public string FX { get; }

        public PlayerFxEvent(string group, string fx)
        {
            Group = group;
            FX = fx;
        }

        public static void TriggerJumpDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "JumpDust"));
        }

        public static void TriggerDoubleJumpDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "DoubleJumpDust"));
        }

        public static void TriggerDashDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "DashDust"));
        }

        public static void TriggerLandDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "LandDust"));
        }

        public static void TriggerSlideDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "SlideDust"));
        }
    }
}