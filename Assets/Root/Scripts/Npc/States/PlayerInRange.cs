// PlayerInRange.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Player;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class PlayerInRange : State<NpcManager>
    {
        [SerializeField]
        private GameObject interactionUI;

        [Range(0, 10)]
        [SerializeField]
        private float rotationSpeed = 5;

        private Transform _lookTarget;

        protected void OnEnable() => interactionUI.SetActive(false);

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            interactionUI.SetActive(true);
            if (rawData.Validate(out PassableDataBase<Transform> data))
                _lookTarget = data.Value;
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (_lookTarget is null) return;
            if(_lookTarget.position - MyOwner.transform.position == Vector3.zero) return;
            var targetRotation = Quaternion.LookRotation(_lookTarget.position - MyOwner.transform.position);
            MyOwner.transform.rotation = Quaternion.RotateTowards(MyOwner.transform.rotation, targetRotation, rotationSpeed);
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            interactionUI.SetActive(false);
            _lookTarget = null;
        }
    }
}