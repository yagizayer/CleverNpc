using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class TriggerForwarder : MonoBehaviour
    {
        [SerializeField]
        private UnityTriggerEvents targetEvent;
        
        [TagField]
        [SerializeField]
        private string targetTag = "Player";

        [SerializeField]
        private UnityEvent<Collider> onTargetEvent;

        #region Unity Methods
        
        private void OnTriggerEnter(Collider other)
        {
            if (targetEvent == UnityTriggerEvents.OnTriggerEnter && other.CompareTag(targetTag))
                onTargetEvent?.Invoke(other);
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (targetEvent == UnityTriggerEvents.OnTriggerStay && other.CompareTag(targetTag))
                onTargetEvent?.Invoke(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (targetEvent == UnityTriggerEvents.OnTriggerExit && other.CompareTag(targetTag))
                onTargetEvent?.Invoke(other);
        }
        
        #endregion
    }
}