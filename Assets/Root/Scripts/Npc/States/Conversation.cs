// Conversation.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Conversation : PlayerInRange
    {
        private Vector2 _currentBehaviourOrientation;

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out ConversationData data)) return;
            base.OnEnterState(stateManager, data.PlayerManager.ToPassableData()); // for PlayerInRange.cs
        }
    }
}