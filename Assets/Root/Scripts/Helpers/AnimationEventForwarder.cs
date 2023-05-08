// AnimationEventForwarder.cs

using UnityEngine;
using UnityEngine.Events;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class AnimationEventForwarder : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onForward1;

        [SerializeField]
        private UnityEvent onForward2;

        [SerializeField]
        private UnityEvent onForward3;

        [SerializeField]
        private UnityEvent onForward4;

        [SerializeField]
        private UnityEvent onForward5;

        public void Forward1() => onForward1?.Invoke();

        public void Forward2() => onForward2?.Invoke();

        public void Forward3() => onForward3?.Invoke();

        public void Forward4() => onForward4?.Invoke();

        public void Forward5() => onForward5?.Invoke();
    }
}