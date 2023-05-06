// GameManager.cs

using System;
using System.Collections;
using UnityEngine;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Managers
{
    public class GameManager : SingletonBase<GameManager>
    {
        // serialized for initialization
        [SerializeField]
        private InputController inputController;

        public static Camera MainCamera { get; private set; }

        private void Awake() => MainCamera = Camera.main;

        public static void ExecuteDelayed(float delay, Action action) =>
            Instance.StartCoroutine(ExecuteDelayedCoroutine(delay, action));

        private static IEnumerator ExecuteDelayedCoroutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}