// NpcManager.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc.States;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : StateManager<NpcManager>
    {
        private void Start() => SetState<Idle>();

        private void Update() => CurrentState.OnUpdateState(this);

        public void OnConversating(IPassableData rawData)
        {
            if (!rawData.Validate(out ConversationData data)) return;
            if (data.NpcManager != this) return;
            SetState<Conversation>(rawData);
        }

        public void OnNpcThinking(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<NpcManager> data)) return;
            if (data.Value != this) return;
            // do nothing
        }
        public void OnNpcAnswering(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<string> data)) return;
            Debug.Log(data.Value);
        }
    }
}