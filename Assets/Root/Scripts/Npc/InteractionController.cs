// InteractionController.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.Npc
{
    public class InteractionController : NpcComponent
    {
        [SerializeField]
        private GameObject interactionUI;

        protected override void OnEnable()
        {
            base.OnEnable();
            interactionUI.SetActive(false);
        }

        private void Update()
        {
            if(NpcManager.MainCamera is null) return;
            interactionUI.transform.LookAt(NpcManager.MainCamera.transform);
        }

        public void OnPlayerEnterRange() => interactionUI.SetActive(true);
        
        public void OnPlayerExitRange() => interactionUI.SetActive(false);
    }
}