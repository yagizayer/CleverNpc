// WWWFormData.cs

using System;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    [Serializable]
    public class WWWFormData
    {
        public string filePath;
        public string fieldName;
        public string mimeType = "audio/wav";
    }
}