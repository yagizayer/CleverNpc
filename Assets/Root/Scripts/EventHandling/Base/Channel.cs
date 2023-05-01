using System.Collections.Generic;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.EventHandling.Base
{
    public class Channel
    {
        private readonly List<Listener> _listeners = new();

        internal void Raise(IPassableData data) => _listeners.Clone().ForEach(listener => listener.OnReceive(data));
        internal void Subscribe(Listener listener) => _listeners.Add(listener);
        internal void Unsubscribe(Listener listener) => _listeners.Remove(listener);
    }
}