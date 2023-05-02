// MovementController.cs

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace YagizAyer.Root.Scripts.Npc
{
    public class MovementController : NpcComponent
    {
        [SerializeField]
        private NavMeshAgent agent;

        public Transform MoveTarget { get; private set; }
        public Transform LookTarget { get; private set; }

        public void StartMoving(Vector3 target) => agent.SetDestination(target);
        public void StartMoving(Transform target) => StartCoroutine(MoveToTarget_CO(target));
        public void StopMoving() => MoveTarget = null;

        public void StartLookingAt(Vector3 target) => transform.LookAt(target);
        public void StartLookingAt(Transform target) => StartCoroutine(LookAtTarget_CO(target));
        public void StopLookingAt() => LookTarget = null;

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

        private IEnumerator LookAtTarget_CO(Transform target)
        {
            LookTarget = target;
            var rotationSpeed = agent.angularSpeed / 300;
            while (LookTarget is not null)
            {
                var targetRotation = Quaternion.LookRotation(LookTarget.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
                yield return null;
            }
        }
    }
}