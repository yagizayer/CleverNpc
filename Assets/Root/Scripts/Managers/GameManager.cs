// GameManager.cs

using System;
using System.Collections;
using UnityEngine;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Player;

namespace YagizAyer.Root.Scripts.Managers
{
    public class GameManager : SingletonBase<GameManager>
    {
        // serialized for initialization
        [SerializeField]
        private InputController inputController;

        [SerializeField]
        private Animator dummyTarget;
        [SerializeField]
        private PlayerManager playerManager;

        public static Camera MainCamera { get; private set; }
        public static Animator DummyTarget { get; private set; }
        public static PlayerManager Player { get; private set; }

        private void Awake()
        {
            MainCamera = Camera.main;
            DummyTarget = dummyTarget;
            Player = playerManager;
        }

        public static void ExecuteDelayed(float delay, Action action) =>
            Instance.StartCoroutine(ExecuteDelayedCoroutine(delay, action));

        private static IEnumerator ExecuteDelayedCoroutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}