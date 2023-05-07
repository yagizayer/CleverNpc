// FriendlyChase.cs

using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class FriendlyChase : Move
    {
        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out NpcAnswerData data)) return;
            base.OnEnterState(stateManager, data.ConversationData.PlayerManager.transform.ToPassableData());
        }
    }
}