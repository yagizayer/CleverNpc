// Idle.cs

using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Move : State<NpcManager>
    {
        [SerializeField]
        internal NavMeshAgent agent;

        public Transform MoveTarget { get; private set; }

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (rawData.Validate(out PassableDataBase<Vector3> v3Data)) agent.SetDestination(v3Data.Value);
            if (rawData.Validate(out PassableDataBase<Transform> trData)) StartCoroutine(MoveToTarget_CO(trData.Value));
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            MoveTarget = null;
        }

        private IEnumerator MoveToTarget_CO(Transform target)
        {
            MoveTarget = target;
            var waitDuration = new WaitForSeconds(0.1f);
            while (MoveTarget is not null)
            {
                agent.SetDestination(MoveTarget.position);
                yield return waitDuration;
            }
        }
    }
}