// GameManager.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        // serialized for initialization
        [SerializeField]
        private InputController inputController;

        public static Camera MainCamera { get; private set; }

        private void Awake() => MainCamera = Camera.main;
    }
}