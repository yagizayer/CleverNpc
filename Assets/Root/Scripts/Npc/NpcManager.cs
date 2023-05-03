// NpcManager.cs

using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc.States;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : StateManager<NpcManager>
    {
        private void Start() => SetState<Idle>();

        private void Update() => CurrentState.OnUpdateState(this);
        
        public void OnConversationStart(IPassableData rawData) => SetState<Conversation>(rawData);
    }
}