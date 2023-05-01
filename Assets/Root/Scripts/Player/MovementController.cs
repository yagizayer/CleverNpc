// PlayerMovementController.cs

using UnityEngine;
using UnityEngine.AI;

namespace YagizAyer.Root.Scripts.Player
{
    public class MovementController : PlayerComponent
    {
        [SerializeField]
        private NavMeshAgent agent;

        private Vector3 _positionOffset;
        private Quaternion _rotation;

        private void Update()
        {
            if (_positionOffset != Vector3.zero) UpdatePosition();
        }

        public void OnCameraInput(Vector2 data) =>
            _rotation = PlayerManager.MainCamera.transform.rotation;

        public void OnMovementInput(Vector2 data) =>
            _positionOffset = new Vector3(data.x, 0, data.y).normalized;

        private void UpdatePosition() => agent.SetDestination(transform.position + _rotation * _positionOffset);
    }
}