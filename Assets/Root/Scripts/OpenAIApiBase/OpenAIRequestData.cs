// OpenAIRequestData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase
{
    public class OpenAIRequestData
    {
        public string Model;
        public string Prompt;
        public float Temperature;
        public int MaxTokens;
        public float TopP;
        public float FrequencyPenalty;
        public float PresencePenalty;
        public string[] Stop;

        public string ToJson()
        {
            var rawData = new OpenAIRequestRawData
            {
                model = Model,
                prompt = Prompt,
                temperature = Temperature,
                max_tokens = MaxTokens,
                top_p = TopP,
                frequency_penalty = FrequencyPenalty,
                presence_penalty = PresencePenalty,
                stop = Stop
            };
            return JsonUtility.ToJson(rawData);
        }

        // for JSON serialization
        [Serializable]
        private class OpenAIRequestRawData
        {
            public string model;
            public string prompt;
            public float temperature;
            public int max_tokens;
            public float top_p;
            public float frequency_penalty;
            public float presence_penalty;
            public string[] stop;
        }
    }
}