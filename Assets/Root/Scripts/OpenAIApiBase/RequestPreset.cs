using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase
{
    [CreateAssetMenu(fileName = "RequestPreset", menuName = "RequestPreset")]
    public class RequestPreset : ScriptableObject
    {
        [SerializeField]
        private Engines engine = Engines.Davinci;

        [Range(0, 250)]
        [SerializeField]
        private int maxTokens = 5;

        [Range(0, 1)]
        [SerializeField]
        private float temperature = 0.9f;

        [Range(0, 1)]
        [SerializeField]
        private float topP = 1;

        [Range(0, 1)]
        [SerializeField]
        private float frequencyPenalty = 0;

        [Range(0, 1)]
        [SerializeField]
        private float presencePenalty = 0;

        [SerializeField]
        private string[] stop = { "\n" };

        public string GetJson(string prompt)
        {
            var result = new OpenAIRequestData
            {
                Model = engine.ToEngineString(),
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