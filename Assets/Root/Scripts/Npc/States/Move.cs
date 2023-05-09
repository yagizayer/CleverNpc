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
        protected Transform MoveTarget;

        private const float TransitionDuration = .25f;
        private float _transitionTimer;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            _agent ??= MyOwner.GetComponentInChildren<NavMeshAgent>();
            if (rawData.Validate(out PassableDataBase<Vector3> v3Data)) _agent.SetDestination(v3Data.Value);
            if (rawData.Validate(out PassableDataBase<Transform> trData)) StartCoroutine(MoveToTarget_CO(trData.Value));
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            float animationValue;
            if (MyOwner.Agent.velocity.sqrMagnitude == 0)
            {
                if (_transitionTimer >= TransitionDuration) return;

                _transitionTimer += Time.deltaTime;
                animationValue = 1 - Mathf.Clamp01(_transitionTimer / TransitionDuration);
                MyOwner.SetAnimationFloat(Animations.Walk.ToAnimationHash(), animationValue);
            }
            else
            {
                animationValue = MyOwner.Agent.velocity.magnitude / MyOwner.Agent.speed;
                MyOwner.SetAnimationFloat(Animations.Walk.ToAnimationHash(), animationValue > .75f ? 1 : animationValue);
            }
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            MoveTarget = null;
        }

        private IEnumerator MoveToTarget_CO(Transform target)
        {
            MoveTarget = target;
            var waitDuration = new WaitForSeconds(.2f);
            var stopDuration = new WaitForSeconds(1f);
            var stopped = true;
            while (MoveTarget is not null)
            {
                var distance = Vector3.Distance(MoveTarget.position, MyOwner.transform.position);
                if (distance < _agent.stoppingDistance + .5f)
                {
                    if (!stopped)
                    {
                        stopped = true;
                        _transitionTimer = 0;
                    }

                    OnReachTarget();
                    yield return stopDuration;
                }
                else stopped = false;

                if (MoveTarget is null) yield break;
                _agent.SetDestination(MoveTarget.position);
                yield return waitDuration;
            }
        }

        protected virtual void OnReachTarget()
        {
        }
    }
}