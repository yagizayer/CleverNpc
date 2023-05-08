// Attack.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Attack : State<NpcManager>
    {
        public Transform Target { get; private set; }

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<Transform> data)) return;
            stateManager.SetAnimationTrigger(Animations.Attack.ToAnimationHash());
            Target = data.Value;
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public void Chase() => MyOwner.SetState<HostileChase>(Target.ToPassableData());
    }
}