using System;
using UnityEngine;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Presets
{
    [CreateAssetMenu(fileName = "CompletionPreset", menuName = "RequestPresets/CompletionRP")]
    public class CompletionPreset : RequestPreset
    {
        [field: SerializeField]
        public override string TargetURL { get; set; } = "https://api.openai.com/v1/completions";

        [field: SerializeField]
        public override ContentType BodyContent { get; set; } = ContentType.ApplicationJson;

        [SerializeField]
        public CompletionEngines completionEngine = CompletionEngines.Davinci;

        [Range(0, 250)]
        [SerializeField]
        public int maxTokens = 50;

        [Range(0, 1)]
        [SerializeField]
        public float temperature = 0.9f;

        [Range(0, 1)]
        [SerializeField]
        public float topP = 1;

        [Range(0, 1)]
        [SerializeField]
        public float frequencyPenalty = 0;

        [Range(0, 1)]
        [SerializeField]
        public float presencePenalty = 0;

        [SerializeField]
        public string[] stop = { };

        public override string GetJson(string prompt)
        {
            var result = new CompletionRequestData
            {
                Model = completionEngine.ToEngineString(),
                Prompt = prompt,
                Temperature = temperature,
                MaxTokens = maxTokens,
                TopP = topP,
                FrequencyPenalty = frequencyPenalty,
                PresencePenalty = presencePenalty,
                Stop = stop
            }.ToJson();
            return result;
        }
    }
}