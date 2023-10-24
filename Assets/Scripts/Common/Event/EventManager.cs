using System;
using System.Collections.Generic;

namespace Common.Event
{
    public static class EventManager
    {
        private static readonly Dictionary<Type, List<IEventListener>> Subscribers = new();
        
        public static void TriggerEvent<T>(T evn) where T : IEvent
        {
            var eventType = typeof(T);
            if (!Subscribers.TryGetValue(eventType, out var listeners))
            {
                return;
            }

            foreach (var listener in listeners)
            {
                (listener as IEventListener<T>)?.OnEventTriggered(evn);
            }
        }

        public static void AddListener<T>(IEventListener<T> listener) where T : IEvent
        {
            var eventType = typeof(T);
            if (Subscribers.TryGetValue(eventType, out var listeners))
            {
                listeners.Add(listener);
            }
            else
            {
                Subscribers[eventType] = new List<IEventListener> { listener };
            }
        }

        public static void RemoveListener<T>(IEventListener<T> listener) where T : IEvent
        {
            var eventType = typeof(T);
            if (!Subscribers.TryGetValue(eventType, out var listeners))
            {
                return;
            }
            listeners.Remove(listener);
        }

        public static void RemoveListeners<T>() where T : IEvent
        {
            var eventType = typeof(T);
            Subscribers.Remove(eventType);
        }

        public static void Clear()
        {
            Subscribers.Clear();
        }
        
    }
}
