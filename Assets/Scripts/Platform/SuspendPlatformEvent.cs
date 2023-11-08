using Common.Event;
using UnityEngine;

namespace Platform
{
    public struct SuspendPlatformEvent : IEvent
    {
        public LayerMask layer;

        public static void TriggerEvent(LayerMask layer)
        {
            SuspendPlatformEvent e;
            e.layer = layer;
            EventManager.TriggerEvent(e);
        }
    }
}