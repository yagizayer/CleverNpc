using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class EventForwarder : MonoBehaviour
    {
        [SerializeField]
        private UnitySpecificEvents targetEvent;
        
        [TagField]
        [SerializeField]
        private string targetTag = "Player";

        [SerializeField]
        private UnityEvent onTargetEventNoParam;

        #region Unity Methods

        private void Awake()
        {
            if (targetEvent == UnitySpecificEvents.Awake)
                onTargetEventNoParam?.Invoke();
        }
        
        private void Start()
        {
            if (targetEvent == UnitySpecificEvents.Start)
                onTargetEventNoParam?.Invoke();
        }
        
        private void Update()
        {
            if (targetEvent == UnitySpecificEvents.Update)
                onTargetEventNoParam?.Invoke();
        }
        
        private void FixedUpdate()
        {
            if (targetEvent == UnitySpecificEvents.FixedUpdate)
                onTargetEventNoParam?.Invoke();
        }
        
        private void LateUpdate()
        {
            if (targetEvent == UnitySpecificEvents.LateUpdate)
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnEnable()
        {
            if (targetEvent == UnitySpecificEvents.OnEnable)
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnDisable()
        {
            if (targetEvent == UnitySpecificEvents.OnDisable)
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnDestroy()
        {
            if (targetEvent == UnitySpecificEvents.OnDestroy)
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (targetEvent == UnitySpecificEvents.OnTriggerEnter && other.CompareTag(targetTag))
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (targetEvent == UnitySpecificEvents.OnTriggerStay && other.CompareTag(targetTag))
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (targetEvent == UnitySpecificEvents.OnTriggerExit && other.CompareTag(targetTag))
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (targetEvent == UnitySpecificEvents.OnCollisionEnter && other.collider.CompareTag(targetTag))
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnCollisionStay(Collision other)
        {
            if (targetEvent == UnitySpecificEvents.OnCollisionStay && other.collider.CompareTag(targetTag))
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (targetEvent == UnitySpecificEvents.OnCollisionExit && other.collider.CompareTag(targetTag))
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnBecameVisible()
        {
            if (targetEvent == UnitySpecificEvents.OnBecameVisible)
                onTargetEventNoParam?.Invoke();
        }
        
        private void OnBecameInvisible()
        {
            if (targetEvent == UnitySpecificEvents.OnBecameInvisible)
                onTargetEventNoParam?.Invoke();
        }

        #endregion
    }
}