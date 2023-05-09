// Move.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Move : State<PlayerManager>
    {
        private Vector3 _positionOffset;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<Vector2> data)) return;
            _positionOffset = new Vector3(data.Value.x, 0, data.Value.y).normalized;
        }

        public override void OnUpdateState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (_positionOffset == Vector3.zero)
            {
                MyOwner.SetState<Idle>();
                return;
            }

            var rotation = GameManager.MainCamera.transform.rotation;
            MyOwner.Agent.SetDestination(transform.position + rotation * _positionOffset);
            var animationValue = MyOwner.Agent.velocity.magnitude / MyOwner.Agent.speed;
            MyOwner.SetAnimationFloat(Animations.Walk.ToAnimationHash(), animationValue > .75f ? 1 : animationValue);
        }

        public override void OnExitState(PlayerManager stateManager, IPassableData rawData = null)
        {
            _positionOffset = Vector3.zero;
        }
    }
}