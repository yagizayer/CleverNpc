using System;
using System.Collections.Generic;
using UnityEngine;

namespace YagizAyer.Root.Scripts.Helpers
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new();

        [SerializeField]
        private List<TValue> values = new();

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            if (keys.Count != values.Count)
                throw new Exception(
                    $"there are {keys.Count} keys and {values.Count} values after deserialization. " +
                    "Make sure that both key and value types are serializable."
                );

            for (var i = 0; i < keys.Count; i++)
                Add(keys[i], values[i]);
        }
    }
}