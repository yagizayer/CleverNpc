// ElevenLabsApiClient.cs

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityWebRequestAwaiter;
using YagizAyer.Root.Scripts.ElevenLabsApiBase.Helpers;

namespace YagizAyer.Root.Scripts.ElevenLabsApiBase
{
    [CreateAssetMenu(fileName = "ElevenLabsApiClient", menuName = "ElevenLabsApiClient")]
    public class ElevenLabsApiClient : ScriptableObject
    {
        [SerializeField]
        private string url = "https://api.elevenlabs.io/v1/text-to-speech/";

        [SerializeField]
        private Voices voice = Voices.Arnold;

        private const int ChunkSize = 1024;

        private readonly string _auth =
            Environment.GetEnvironmentVariable("XI_API_KEY", EnvironmentVariableTarget.User);

        internal async void RequestAsync(string textToVocalize, Action<AudioClip> onComplete)
        {
            var request = UnityWebRequest.Post(url + voice.ToVoiceID(), "POST");
            request.SetRequestHeader("Accept", "audio/mpeg");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("xi-api-key", _auth);

            var data = new RequestBody
            {
                text = textToVocalize,
                model_id = "eleven_monolingual_v1"
            };

            var json = JsonUtility.ToJson(data);
            var postData = System.Text.Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                return;
            }

            var responseBytes = request.downloadHandler.data;
            await using var fileStream = new FileStream("Assets/Resources/Audio/NpcAnswer.mp3", FileMode.Create);

            var offset = 0;
            while (offset < responseBytes.Length)
            {
                var chunkSize = Mathf.Min(ChunkSize, responseBytes.Length - offset);
                await fileStream.WriteAsync(responseBytes, offset, chunkSize);

                offset += chunkSize;
            }

            var audioClip = Resources.Load<AudioClip>("Audio/NpcAnswer");
            onComplete(audioClip);
        }

        private class RequestBody
        {
            public string text;
            public string model_id;
        }
    }
}