using System;
using System.Collections.Generic;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.EventHandling.Base
{
    public class Listener
    {
        private readonly List<Action<IPassableData>> _actions = new();

        public Listener(Action<IPassableData> action) => _actions.Add(action);

        public void OnReceive(IPassableData data) => _actions.ForEach(action => action(data));
    }
}