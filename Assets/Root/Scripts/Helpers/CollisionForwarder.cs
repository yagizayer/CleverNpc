using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class CollisionForwarder : MonoBehaviour
    {
        [SerializeField]
        private UnityCollisionEvents targetEvent;
        
        [TagField]
        [SerializeField]
        private string targetTag = "Player";

        [SerializeField]
        private UnityEvent<Collision> onTargetEvent;

        #region Unity Methods
        
        private void OnCollisionEnter(Collision other)
        {
            if (targetEvent == UnityCollisionEvents.OnCollisionEnter && other.collider.CompareTag(targetTag))
                onTargetEvent?.Invoke(other);
        }
        
        private void OnCollisionStay(Collision other)
        {
            if (targetEvent == UnityCollisionEvents.OnCollisionStay && other.collider.CompareTag(targetTag))
                onTargetEvent?.Invoke(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (targetEvent == UnityCollisionEvents.OnCollisionExit && other.collider.CompareTag(targetTag))
                onTargetEvent?.Invoke(other);
        }
        
        #endregion
    }
}