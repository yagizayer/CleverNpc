// Conversation.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Conversation : PlayerInRange
    {
        private Vector2 _currentBehaviourOrientation;

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            base.OnEnterState(stateManager, GameManager.Player.transform.ToPassableData());
        }
    }
}