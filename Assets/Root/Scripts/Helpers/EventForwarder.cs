using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class EventForwarder : MonoBehaviour
    {
        [SerializeField]
        private UnityNoParamEvents targetEvent;
        
        [TagField]
        [SerializeField]
        private string targetTag = "Player";

        [SerializeField]
        private UnityEvent onTargetEvent;

        #region Unity Methods

        private void Awake()
        {
            if (targetEvent == UnityNoParamEvents.Awake)
                onTargetEvent?.Invoke();
        }
        
        private void Start()
        {
            if (targetEvent == UnityNoParamEvents.Start)
                onTargetEvent?.Invoke();
        }
        
        private void Update()
        {
            if (targetEvent == UnityNoParamEvents.Update)
                onTargetEvent?.Invoke();
        }
        
        private void FixedUpdate()
        {
            if (targetEvent == UnityNoParamEvents.FixedUpdate)
                onTargetEvent?.Invoke();
        }
        
        private void LateUpdate()
        {
            if (targetEvent == UnityNoParamEvents.LateUpdate)
                onTargetEvent?.Invoke();
        }
        
        private void OnEnable()
        {
            if (targetEvent == UnityNoParamEvents.OnEnable)
                onTargetEvent?.Invoke();
        }
        
        private void OnDisable()
        {
            if (targetEvent == UnityNoParamEvents.OnDisable)
                onTargetEvent?.Invoke();
        }
        
        private void OnDestroy()
        {
            if (targetEvent == UnityNoParamEvents.OnDestroy)
                onTargetEvent?.Invoke();
        }
        
        private void OnBecameVisible()
        {
            if (targetEvent == UnityNoParamEvents.OnBecameVisible)
                onTargetEvent?.Invoke();
        }
        
        private void OnBecameInvisible()
        {
            if (targetEvent == UnityNoParamEvents.OnBecameInvisible)
                onTargetEvent?.Invoke();
        }

        #endregion
    }
}