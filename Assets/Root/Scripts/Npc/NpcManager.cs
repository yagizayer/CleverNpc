// NpcManager.cs

using System;
using UnityEngine;
using System.Collections.Generic;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : MonoBehaviour
    {
        [SerializeField]
        private NpcComponent[] controllers;

        internal Camera MainCamera;
        private Dictionary<Type, NpcComponent> ControllersDict { get; } = new();

        private void OnEnable()
        {
            controllers = GetComponentsInChildren<NpcComponent>();
            MainCamera = Camera.main;
            foreach (var controller in controllers) ControllersDict.Add(controller.GetType(), controller);
        }

        public void OnPlayerEnterRange(Transform player)
        {
            if (ControllersDict.TryGetValue(
                    typeof(InteractionController),
                    out InteractionController interactionController)
               ) interactionController!.OnPlayerEnterRange();
            if (ControllersDict.TryGetValue(
                    typeof(MovementController),
                    out MovementController movementController)
               ) movementController!.StartLookingAt(player);
        }

        public void OnPlayerExitRange(Transform player)
        {
            if (ControllersDict.TryGetValue(
                    typeof(InteractionController),
                    out InteractionController interactionController)
               ) interactionController!.OnPlayerExitRange();
            if (ControllersDict.TryGetValue(
                    typeof(MovementController),
                    out MovementController movementController)
               ) movementController!.StopLookingAt();
        }
    }
}