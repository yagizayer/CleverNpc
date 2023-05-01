// PlayerManager.cs

using System;
using System.Linq;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerComponent[] controllers;

        internal Camera MainCamera;

        private void OnEnable()
        {
            controllers = GetComponentsInChildren<PlayerComponent>();
            MainCamera = Camera.main;
        }

        public void OnCameraInput(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<Vector2> data)) return;
            var controller =
                controllers.FirstOrDefault(component => component is MovementController) as MovementController;
            controller!.OnCameraInput(data.Value);
        }

        public void OnMovementInput(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<Vector2> data)) return;
            var controller =
                controllers.FirstOrDefault(component => component is MovementController) as MovementController;
            controller!.OnMovementInput(data.Value);
        }
    }
}