// Move.cs

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Move : State<NpcManager>
    {
        private NavMeshAgent _agent;

        [SerializeField]
        private Transform moveTarget;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            _agent ??= MyOwner.GetComponentInChildren<NavMeshAgent>();
            if (rawData.Validate(out PassableDataBase<Vector3> v3Data)) _agent.SetDestination(v3Data.Value);
            if (rawData.Validate(out PassableDataBase<Transform> trData)) StartCoroutine(MoveToTarget_CO(trData.Value));
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            moveTarget = null;
        }

        private IEnumerator MoveToTarget_CO(Transform target)
        {
            moveTarget = target;
            var waitDuration = new WaitForSeconds(0.2f);
            while (moveTarget is not null)
            {
                _agent.SetDestination(moveTarget.position);
                yield return waitDuration;
            }
        }
    }
}