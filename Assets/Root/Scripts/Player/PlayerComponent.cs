// PlayerComponent.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Player
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        protected PlayerManager PlayerManager;
        protected void OnEnable() => PlayerManager = GetComponentInParent<PlayerManager>();

        protected void OnDisable() => PlayerManager = null;

        public abstract void OnCameraInput(Vector2 mouseDelta);
        public abstract void OnMovementInput(Vector2 movementInput);

        public abstract void OnInteractInput(bool isInteractionCalled);

        public abstract void OnNextDialogInput(bool isNextDialogCalled);
    }
}