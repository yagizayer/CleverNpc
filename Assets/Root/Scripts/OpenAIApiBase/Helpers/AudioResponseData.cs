// OpenAIResponseData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    /*
        {
          "text": "Imagine the wildest idea that you've ever had, and you're curious about how it might scale to something that's a 100, a 1,000 times bigger. This is a place where you can get to do that."
        }
     */
    public class AudioResponseData
    {
        public string Text;
        
        public static AudioResponseData FromJson(string json)
        {
            var rawData = JsonUtility.FromJson<AudioResponseRawData>(json);
            var result = new AudioResponseData
            {
                Text = rawData.text
            };
            return result;
        }

        // for JSON serialization
        [Serializable]
        private class AudioResponseRawData
        {
            public string text;
        }
    }
}