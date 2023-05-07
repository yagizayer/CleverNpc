// InputController.cs

using UnityEngine;
using UnityEngine.InputSystem;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Managers
{
    [CreateAssetMenu(fileName = "InputController", menuName = "Controllers/InputController", order = 0)]
    public class InputController : ScriptableObject, InputActions.IGameplayActions
    {
        private InputActions _inputActions;

        private void OnEnable()
        {
            _inputActions = new InputActions();
            _inputActions.Gameplay.SetCallbacks(this);

            _inputActions.Enable();
        }

        private void OnDisable() => _inputActions.Disable();

        public void OnMovement(InputAction.CallbackContext context) =>
            Channels.Movement.Raise(context.ReadValue<Vector2>().ToPassableData());

        public void OnInteract(InputAction.CallbackContext context)
        {
            // held down = true, released = false
            if (context.started) return;
            Channels.Interact.Raise(context.performed.ToPassableData());
        }

        public void OnRecord(InputAction.CallbackContext context)
        {
            // held down = true, released = false
            if (context.started) return;
            Channels.Record.Raise(context.performed.ToPassableData());
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            // held down = true, released = false
            if (context.started) return;
            Channels.Cancel.Raise(context.performed.ToPassableData());
        }
    }
}