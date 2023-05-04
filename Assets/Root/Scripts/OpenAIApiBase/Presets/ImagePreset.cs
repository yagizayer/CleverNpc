using UnityEngine;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Presets
{
    [CreateAssetMenu(fileName = "ImagePreset", menuName = "RequestPresets/ImageRP")]
    public class ImagePreset : RequestPreset
    {
        [field: SerializeField]
        public override string TargetURL { get; set; } = "https://api.openai.com/v1/images/generations";

        [field: SerializeField]
        public override ContentType BodyContent { get; set; } = ContentType.ApplicationJson;

        [SerializeField]
        private Vector2 size = new(1024, 1024);

        public override string GetJson(string input)
        {
            var result = new ImageRequestData
            {
                Prompt = input,
                Size = size
            }.ToJson();
            return result;
        }
    }
}