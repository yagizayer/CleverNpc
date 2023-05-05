// AudioRequestData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    /*
       {
          "file": "audio.mp3",
          "model": "whisper-1"
        }
     */
    public class AudioRequestData
    {
        public string File;
        public AudioEngines Model;

        public string ToJson()
        {
            var rawData = new AudioRequestRawData
            {
                file = File,
                model = Model.ToEngineString()
            };
            return JsonUtility.ToJson(rawData);
        }

        // for JSON serialization
        [Serializable]
        private class AudioRequestRawData
        {
            public string file;
            public string model;
        }
    }
}