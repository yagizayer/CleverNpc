// Attack.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Attack : State<NpcManager>
    {
        public Transform Target { get; private set; }

        private Quaternion _targetRotation;
        private const float RotationSpeed = 5f;

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<Transform> data)) return;
            stateManager.SetAnimationTrigger(Animations.Attack.ToAnimationHash());
            Target = data.Value;
            _targetRotation = Quaternion.LookRotation(Target.position - MyOwner.transform.position);
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            // called at every frame
            if (Target == null) return;
            MyOwner.transform.rotation = Quaternion.Slerp(MyOwner.transform.rotation, _targetRotation,
                RotationSpeed * Time.deltaTime);
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public void Chase() => MyOwner.SetState<HostileChase>(Target.ToPassableData());
    }
}