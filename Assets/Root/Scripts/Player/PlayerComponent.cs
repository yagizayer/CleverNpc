// PlayerComponent.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Player
{
    public abstract class PlayerComponent : MonoBehaviour
    {
        protected PlayerManager PlayerManager;
        protected virtual void OnEnable() => PlayerManager = GetComponentInParent<PlayerManager>();

        protected virtual void OnDisable() => PlayerManager = null;

        public abstract void OnCameraInput(Vector2 data);
        public abstract void OnMovementInput(Vector2 data);

        public abstract void OnInteractInput(bool data);

        public abstract void OnNextDialogInput(bool data);
    }
}