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

        public override void OnCameraInput(Vector2 mouseDelta)
        {
            _rotation = PlayerManager.mainCamera.transform.forward.OnPlane();
        }

        public override void OnMovementInput(Vector2 movementInput)
        {
            var movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;

            _positionOffset = movementDirection * (movementSpeed * Time.deltaTime);
        }

        public override void OnInteractInput(bool isInteractionCalled)
        {
            Debug.Log(isInteractionCalled);
        }

        public override void OnNextDialogInput(bool isNextDialogCalled)
        {
            Debug.Log(isNextDialogCalled);
        }

        private void UpdateRotation() =>
            transform.forward = Vector3.Lerp(transform.forward, _rotation, rotationSpeed * Time.deltaTime);

        private void UpdatePosition() => transform.position += transform.rotation * _positionOffset;
    }
}