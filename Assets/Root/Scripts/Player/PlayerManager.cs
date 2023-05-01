// PlayerManager.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        public Camera mainCamera;

        [SerializeField]
        private PlayerComponent[] controllers;

        private void OnEnable()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            
            controllers = GetComponentsInChildren<PlayerComponent>();
        }

        public void OnCameraInput(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<Vector2> data)) return;
            foreach (var controller in controllers) controller.OnCameraInput(data.Value);
        }

        public void OnMovementInput(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<Vector2> data)) return;
            foreach (var controller in controllers) controller.OnMovementInput(data.Value);
        }
        
        public void OnInteractInput(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<bool> data)) return;
            foreach (var controller in controllers) controller.OnInteractInput(data.Value);
        }
        
        public void OnNextDialogInput(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<bool> data)) return;
            foreach (var controller in controllers) controller.OnNextDialogInput(data.Value);
        }
    }
}