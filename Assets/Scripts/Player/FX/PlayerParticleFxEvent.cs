using Common.Event;
using UnityEngine;

namespace Player.FX
{
    public struct PlayerParticleFxEvent : IEvent
    {

        public string FX { get; private set; }
        public object Param { get; private set; }

        public PlayerParticleFxEvent(string fx, object param)
        {
            FX = fx;
            Param = param;
        }

        public T GetParam<T>() where T : class
        {
            return Param as T;
        }



        public class RunDust
        {
            public int direction;
            public bool start;

            public void TriggerEvent()
            {
                EventManager.TriggerEvent(new PlayerParticleFxEvent(nameof(RunDust), this));
            }
        }



    }
}