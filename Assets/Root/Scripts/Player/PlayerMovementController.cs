// PlayerMovementController.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Player
{
    public class PlayerMovementController : PlayerComponent
    {
        [Range(0, 200)]
        [SerializeField]
        private float movementSpeed = 10f;

        [Range(0, 200)]
        [SerializeField]
        private float rotationSpeed = 10f;

        private Vector3 _positionOffset;
        private Vector3 _rotation;

        private void Update()
        {
            UpdateRotation();
            UpdatePosition();
        }

        public override void OnCameraInput(Vector2 data)
        {
            _rotation = PlayerManager.mainCamera.transform.forward.OnPlane();
        }

        public override void OnMovementInput(Vector2 data)
        {
            var movementDirection = new Vector3(data.x, 0, data.y).normalized;

            _positionOffset = movementDirection * (movementSpeed * Time.deltaTime);
        }

        public override void OnInteractInput(bool data)
        {
            Debug.Log(data);
        }

        public override void OnNextDialogInput(bool data)
        {
            Debug.Log(data);
        }

        private void UpdateRotation() =>
            transform.forward = Vector3.Lerp(transform.forward, _rotation, rotationSpeed * Time.deltaTime);

        private void UpdatePosition() => transform.position += transform.rotation * _positionOffset;
    }
}