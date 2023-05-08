// HostileChase.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class HostileChase : Move
    {
        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<Transform> data)) return;
            Channels.CancelConversating.Raise(MyOwner.ToPassableData());
            base.OnEnterState(stateManager, rawData);
        }

        protected override void OnReachTarget(Transform target) =>
            GameManager.ExecuteDelayed(1f, () =>
            {
                if (MyOwner.CurrentState is HostileChase)
                    MyOwner.SetState<Attack>(target.ToPassableData());
            });
    }
}