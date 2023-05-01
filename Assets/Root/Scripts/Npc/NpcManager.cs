// NpcManager.cs

using System;
using System.Linq;
using UnityEngine;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : MonoBehaviour
    {
        [SerializeField]
        private NpcComponent[] controllers;

        internal Camera MainCamera;

        private void OnEnable()
        {
            controllers = GetComponentsInChildren<NpcComponent>();
            MainCamera = Camera.main;
        }

        public void OnPlayerEnterRange()
        {
            var controller = controllers.FirstOrDefault(component => component is InteractionController) as InteractionController;
            controller!.OnPlayerEnterRange();
        }
        
        public void OnPlayerExitRange()
        {
            var controller = controllers.FirstOrDefault(component => component is InteractionController) as InteractionController;
            controller!.OnPlayerExitRange();
        }
    }
}