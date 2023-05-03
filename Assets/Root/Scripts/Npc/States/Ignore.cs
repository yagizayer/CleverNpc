// FriendlyChase.cs

using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Ignore : State<NpcManager>
    {
        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }
    }
}