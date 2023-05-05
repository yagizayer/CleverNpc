// ConversationResponseData.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    [Serializable]
    public class ConversationResponseData : IPassableData
    {
        public float positivity;
        public float friendliness;

        public static ConversationResponseData FromJson(string json)
        {
            var rawData = JsonUtility.FromJson<RawConversationResponseData>(json);
            return new ConversationResponseData
            {
                positivity = float.Parse(rawData.positivity.Replace('.', ',').Trim()),
                friendliness = float.Parse(rawData.friendliness.Replace('.', ',').Trim())
            };
        }

        // for JSON serialization
        [Serializable]
        private class RawConversationResponseData
        {
            public string positivity;
            public string friendliness;
        }
    }
}