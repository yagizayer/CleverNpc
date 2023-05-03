// GameManager.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static Camera MainCamera { get; private set; }
        
        private void Awake() => MainCamera = Camera.main;
    }
}