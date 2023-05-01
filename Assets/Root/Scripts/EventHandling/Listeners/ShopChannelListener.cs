using UnityEngine;
using UnityEngine.Events;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.EventHandling.Listeners
{
    public sealed class ShopChannelListener : MonoBehaviour
    {
        [SerializeField]
        private ShopChannels channel;
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
    }
}
