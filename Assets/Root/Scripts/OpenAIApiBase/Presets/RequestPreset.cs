// RequestPreset.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Presets
{
    public abstract class RequestPreset : ScriptableObject
    {
        public abstract string TargetURL { get; set; }
        public abstract ContentType BodyContent { get; set; }
        public abstract string GetJson(string input);

        public enum ContentType
        {
            ApplicationJson,
            TextPlain,
            MultipartFormData,
            ApplicationXWwwFormUrlencoded
        }
    }
}