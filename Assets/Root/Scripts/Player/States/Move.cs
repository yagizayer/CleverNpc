// Move.cs

using UnityEngine;
using UnityEngine.AI;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Move : State<PlayerManager>
    {
        [SerializeField]
        private NavMeshAgent agent;

        private Vector3 _positionOffset;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<Vector2> data)) return;
            _positionOffset = new Vector3(data.Value.x, 0, data.Value.y).normalized;
        }

        public override void OnUpdateState(PlayerManager stateManager, IPassableData rawData = null)
        {
            OnEnterState(stateManager, rawData);
            
            if (_positionOffset == Vector3.zero)
            {
                MyOwner.SetState<Idle>();
                return;
            }
            var rotation = GameManager.MainCamera.transform.rotation;
            agent.SetDestination(transform.position + rotation * _positionOffset);
        }

        public override void OnExitState(PlayerManager stateManager, IPassableData rawData = null)
        {
            _positionOffset = Vector3.zero;
        }
    }
}