// Idle.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Npc;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Idle : State<PlayerManager>
    {
        [Range(0, 10)]
        [SerializeField]
        private float rotationSpeed = 5;

        private Transform _lookTarget;
        private const float TransitionDuration = .25f;
        private float _transitionTimer;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (rawData.Validate(out PassableDataBase<NpcManager> data))
                _lookTarget = data.Value.transform;
            _transitionTimer = 0;
        }

        public override void OnUpdateState(PlayerManager stateManager, IPassableData rawData = null)
        {
            _transitionTimer += Time.deltaTime;
            var animationValue = Mathf.Clamp01(1 - _transitionTimer / TransitionDuration);
            MyOwner.SetAnimationFloat(Animations.Walk.ToAnimationHash(), animationValue);
            
            if (_lookTarget is null) return;
            if (_lookTarget.position - MyOwner.transform.position == Vector3.zero) return;
            var targetRotation = Quaternion.LookRotation(_lookTarget.position - MyOwner.transform.position);
            MyOwner.transform.rotation =
                Quaternion.RotateTowards(MyOwner.transform.rotation, targetRotation, rotationSpeed);
        }

        public override void OnExitState(PlayerManager stateManager, IPassableData rawData = null)
        {
            _lookTarget = null;
        }
    }
}