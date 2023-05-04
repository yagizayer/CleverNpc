// IFormData.cs

using System.Collections.Generic;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    public interface IWWWFormDataWrapper
    {
        public SerializableDictionary<string, string> Fields { get; set; }
        public SerializableDictionary<string, WWWFormData> FilePaths { get; set; } 
    }
}