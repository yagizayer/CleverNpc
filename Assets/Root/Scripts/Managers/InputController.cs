// InputController.cs

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Managers
{
    [CreateAssetMenu(fileName = "InputController", menuName = "Controllers/InputController", order = 0)]
    public class InputController : ScriptableObject, InputActions.IGameplayActions, InputActions.IMenuActions
    {
        private InputActions _inputActions;

        private void OnEnable()
        {
            _inputActions = new InputActions();
            _inputActions.Gameplay.SetCallbacks(this);
            _inputActions.Menu.SetCallbacks(this);

            _inputActions.Enable();
        }

        private void OnDisable() => _inputActions.Disable();

        public void OnMovement(InputAction.CallbackContext context) =>
            Channels.GameplayMovement.Raise(context.ReadValue<Vector2>().ToPassableData());

        public void OnInteract(InputAction.CallbackContext context) =>
            Channels.GameplayInteract.Raise(context.started.ToPassableData());

        public void OnNext(InputAction.CallbackContext context) =>
            Channels.MenuNext.Raise(context.started.ToPassableData());
    }
}