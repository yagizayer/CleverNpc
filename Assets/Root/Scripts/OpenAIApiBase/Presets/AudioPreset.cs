using System.Collections.Generic;
using UnityEngine;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Presets
{
    [CreateAssetMenu(fileName = "AudioPreset", menuName = "RequestPresets/AudioRP")]
    public class AudioPreset : RequestPreset, IWWWFormDataWrapper
    {
        [field: SerializeField]
        public override string TargetURL { get; set; } = "https://api.openai.com/v1/audio/transcriptions";

        [field: SerializeField] public SerializableDictionary<string, string> Fields { get; set; } = new();
        [field: SerializeField] public SerializableDictionary<string, WWWFormData> FilePaths { get; set; } = new();

        [field: SerializeField] public override ContentType BodyContent { get; set; } = ContentType.MultipartFormData;

        [SerializeField]
        private AudioEngines audioEngine = AudioEngines.Whisper1;

        public override string GetJson(string input)
        {
            var result = new AudioRequestData
            {
                File = input,
                Model = audioEngine
            }.ToJson();
            return result;
        }
    }
}