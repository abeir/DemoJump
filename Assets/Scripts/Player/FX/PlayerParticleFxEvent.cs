using Common.Event;

namespace Player.FX
{
    public class PlayerParticleFxEvent : IEvent
    {
        public static readonly string RunDust = "RunDust";

        public string FX { get; private set; }
        public int Direction { get; private set; }
        public bool Start { get; private set; }

        public PlayerParticleFxEvent(string fx, int direction, bool start)
        {
            FX = fx;
            Direction = direction;
            Start = start;
        }

        public static void TriggerRunDust(int direction, bool start)
        {
            EventManager.TriggerEvent(new PlayerParticleFxEvent(RunDust, direction, start));
        }


    }
}