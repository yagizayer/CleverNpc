using UnityEngine;
using UnityEngine.Events;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.EventHandling.Listeners
{
    public sealed class ChannelListener : MonoBehaviour
    {
        [SerializeField]
        private Channels channel;

        [SerializeField]
        private UnityEvent<IPassableData> onEventRaised;

        private Listener _listener;

        private void OnEnable()
        {
            _listener = new Listener(onEventRaised.Invoke);
            channel.Subscribe(_listener);
        }

        private void OnDisable() => channel.Unsubscribe(_listener);

        [ContextMenu("Debug Raise")]
        public void DebugRaise() => onEventRaised.Invoke(null);

        public void ConsoleLog(IPassableData data) => data.ConsoleLog();
    }
}