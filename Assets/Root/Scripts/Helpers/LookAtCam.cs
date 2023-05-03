// LookAtCam.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class LookAtCam : MonoBehaviour
    {
        private Camera _camera;

        private void OnEnable() => _camera = Camera.main;
        
        private void Update()
        {
            if (_camera is null) return;
            transform.LookAt(_camera.transform);
        }
    }
}