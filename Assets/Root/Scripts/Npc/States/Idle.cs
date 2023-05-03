// Idle.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Idle : State<NpcManager>
    {
        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            // do nothing
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            // do nothing
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            // do nothing
        }
    }
}