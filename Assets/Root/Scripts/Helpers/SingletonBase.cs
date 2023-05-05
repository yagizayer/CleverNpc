// SingletonBase.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Helpers
{
    public abstract class SingletonBase<T> : MonoBehaviour where T : class
    {
        protected static T Instance { get; set; }

        public virtual void OnEnable()
        {
            if (Instance == null)
                Instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}