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
        private Transform _moveTarget;

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
            _moveTarget = null;
        }

        private IEnumerator MoveToTarget_CO(Transform target)
        {
            _moveTarget = target;
            var waitDuration = new WaitForSeconds(.2f);
            var stopDuration = new WaitForSeconds(1f);
            while (_moveTarget is not null)
            {
                var distance = Vector3.Distance(_moveTarget.position, MyOwner.transform.position);
                if (distance < _agent.stoppingDistance)
                {
                    OnReachTarget(target);
                    yield return stopDuration;
                }
                else
                    MyOwner.PlayAnimation(Animations.Walk.ToAnimationHash());

                if (_moveTarget is null) yield break;
                _agent.SetDestination(_moveTarget.position);
                yield return waitDuration;
            }
        }

        protected virtual void OnReachTarget(Transform target)
        {
            MyOwner.PlayAnimation(Animations.Idle.ToAnimationHash());
        }
    }
}