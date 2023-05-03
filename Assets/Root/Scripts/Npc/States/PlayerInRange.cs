// Idle.cs

using System.Collections;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class PlayerInRange : State<NpcManager>
    {
        [SerializeField]
        private GameObject interactionUI;

        [Range(0, 10)]
        [SerializeField]
        private float rotationSpeed;

        public Transform LookTarget { get; private set; }

        protected void OnEnable() => interactionUI.SetActive(false);

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            interactionUI.SetActive(true);
            if (rawData.Validate(out PassableDataBase<Transform> data))
                LookTarget = data.Value;
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (LookTarget is null) return;
            var targetRotation = Quaternion.LookRotation(LookTarget.position - transform.position);
            MyOwner.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            interactionUI.SetActive(false);
            LookTarget = null;
        }
    }
}