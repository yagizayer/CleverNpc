// OpenAIRequestData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    /*
       {
            "model": "text-davinci-003",
            "prompt": "Say this is a test.",
            "temperature": 0.2,
            "max_tokens": 50,
            "top_p": 1,
            "frequency_penalty": 0.33,
            "presence_penalty": 1,
            "stop": [
                "User:",
                "\n"
            ]
        }
     */
    public class CompletionRequestData
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
            var rawData = new CompletionRequestRawData
            {
                model = Model,
                prompt = Prompt,
                temperature = Temperature,
                max_tokens = MaxTokens,
                top_p = TopP,
                frequency_penalty = FrequencyPenalty,
                presence_penalty = PresencePenalty
            };
            if(Stop is { Length: > 0 }) rawData.stop = Stop;
            return JsonUtility.ToJson(rawData).Replace(",\"stop\":[]","");
        }

        // for JSON serialization
        [Serializable]
        private class CompletionRequestRawData
        {
            public string model;
            public string prompt;
            public float temperature;
            public int max_tokens;
            public float top_p;
            public float frequency_penalty;
            public float presence_penalty;
            public string[] stop = null;
        }
    }
}