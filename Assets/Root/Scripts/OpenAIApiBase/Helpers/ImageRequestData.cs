// ImageRequestData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    /*
       {
            "prompt": "A cute baby sea otter",
            "size": "1024x1024"
        }
     */
    public class ImageRequestData
    {
        public string Prompt;
        public Vector2 Size;

        public string ToJson()
        {
            var rawData = new ImageRequestRawData
            {
                prompt = Prompt,
                size = $"{Size.x}x{Size.y}"
            };
            return JsonUtility.ToJson(rawData);
        }

        // for JSON serialization
        [Serializable]
        private class ImageRequestRawData
        {
            public string prompt;
            public string size;
        }
    }
}