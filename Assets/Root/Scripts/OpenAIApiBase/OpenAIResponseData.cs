// OpenAIResponseData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase
{
    public class OpenAIResponseData
    {
        public string Id;
        public int Created;
        public string Model;
        public Choice[] Choices;
        public Usage Usages;

        public class Choice
        {
            public string Text;
            public int Index;
            public string FinishReason;
        }

        public class Usage
        {
            public int PromptTokens;
            public int CompletionTokens;
            public int TotalTokens;
        }

        public static OpenAIResponseData FromJson(string json)
        {
            var rawData = JsonUtility.FromJson<OpenAIResponseRawData>(json);
            var choices = new Choice[rawData.choices.Length];
            for (var i = 0; i < rawData.choices.Length; i++)
            {
                var rawChoice = rawData.choices[i];
                choices[i] = new Choice
                {
                    Text = rawChoice.text,
                    Index = rawChoice.index,
                    FinishReason = rawChoice.finish_reason
                };
            }
            var usages = new Usage
            {
                PromptTokens = rawData.usage.prompt_tokens,
                CompletionTokens = rawData.usage.completion_tokens,
                TotalTokens = rawData.usage.total_tokens
            };
            var result = new OpenAIResponseData
            {
                Id = rawData.id,
                Created = rawData.created,
                Model = rawData.model,
                Choices = choices,
                Usages = usages
            };
            return result;
        }

        
        // for JSON serialization
        [Serializable]
        private class OpenAIResponseRawData
        {
            public string id;
            public int created;
            public string model;
            public RawChoice[] choices;
            public RawUsage usage;
        }

        [Serializable]
        private class RawChoice
        {
            public string text;
            public int index;
            public string logprobs;
            public string finish_reason;
        }

        [Serializable]
        private class RawUsage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }
    }
}