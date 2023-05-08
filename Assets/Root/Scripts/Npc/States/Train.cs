// Train.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Train : Move
    {
        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<Transform> data)) return;
            Channels.CancelConversating.Raise(MyOwner.ToPassableData());
            base.OnEnterState(stateManager, data.Value.ToPassableData());
        }

        protected override void OnReachTarget(Transform target) =>
            MyOwner.SetState<Attack>(target.ToPassableData());
    }
}