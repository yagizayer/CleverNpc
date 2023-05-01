// InteractionController.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Npc
{
    public class InteractionController : NpcComponent
    {
        [SerializeField]
        private GameObject interactionUI;

        private bool _isPlayerInRange;
        private bool IsInteractable => _isPlayerInRange;
        private Camera _mainCamera;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            interactionUI.SetActive(false);
            _mainCamera = Camera.main;
        }

        private void Update() => interactionUI.transform.forward = _mainCamera.transform.forward;

        public void SetIsPlayerInRange(bool isPlayerInRange)
        {
            _isPlayerInRange = isPlayerInRange;
            interactionUI.SetActive(IsInteractable);
        }
        
    }
}