// CompletionResponseData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    
    /*
        {
            "id": "cmpl-7CP8vzBiROvM9QohXacZDb3DBTIEt",
            "object": "text_completion",
            "created": 1683190941,
            "model": "text-davinci-003",
            "choices": [
                {
                    "text": "This is indeed a test.",
                    "index": 0,
                    "logprobs": null,
                    "finish_reason": "stop"
                }
            ],
            "usage": {
                "prompt_tokens": 244,
                "completion_tokens": 11,
                "total_tokens": 255
            }
        }
     */
    public class CompletionResponseData
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

        public static CompletionResponseData FromJson(string json)
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
            var result = new CompletionResponseData
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