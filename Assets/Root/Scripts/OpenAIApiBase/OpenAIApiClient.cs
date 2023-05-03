using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityWebRequestAwaiter;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.OpenAIApiBase
{
    [CreateAssetMenu(fileName = "OpenAIApiClient", menuName = "OpenAIApiClient")]
    public class OpenAIApiClient : ScriptableObject
    {
        private const string TargetURL = "https://api.openai.com/v1/completions";

        private readonly string _auth =
            $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User)}";

        public IEnumerator Request_CO(string prompt, RequestPreset preset, Action<string> onComplete)
        {
            var request = PrepareRequest(prompt, preset);

            var result = request.SendWebRequest();
            while (!result.isDone)
                yield return null;

            var json = request.downloadHandler.text.ToJson();
            onComplete(json);
        }

        public async void RequestAsync(string prompt, RequestPreset preset, Action<string> onComplete)
        {
            var request = PrepareRequest(prompt, preset);

            await request.SendWebRequest();
            var json = request.downloadHandler.text.ToJson();
            onComplete(json);
        }

        private UnityWebRequest PrepareRequest(string prompt, RequestPreset preset)
        {
            var body = preset.GetJson(prompt);
            var data = System.Text.Encoding.UTF8.GetBytes(body);
            var request = new UnityWebRequest(TargetURL, "POST");
            
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", _auth);
            
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();

            return request;
        }
    }
}