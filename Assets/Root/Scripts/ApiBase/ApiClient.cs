using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityWebRequestAwaiter;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.ApiBase
{
    [CreateAssetMenu(fileName = "ApiClient", menuName = "ApiClient")]
    public class ApiClient : ScriptableObject
    {
        [SerializeField]
        private Engines engine = Engines.Davinci;

        [SerializeField]
        private string targetURL;

        private void OnEnable() => targetURL = $"https://api.openai.com/v1/engines/{engine.ToEngineString()}/completions";

#if UNITY_EDITOR
        private void OnValidate() => OnEnable();
#endif

        private readonly string _auth =
            $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User)}";

        public IEnumerator Request_CO(string prompt, RequestPreset preset, Action<string> onComplete)
        {
            var body = preset.GetJson(prompt);
            var data = System.Text.Encoding.UTF8.GetBytes(body);
            var request = new UnityWebRequest(targetURL, "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", _auth);
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();

            var result = request.SendWebRequest();
            while (!result.isDone)
                yield return null;

            var json = request.downloadHandler.text.ToJson();
            onComplete(json);
        }

        public async void RequestAsync(string prompt, RequestPreset preset, Action<string> onComplete)
        {
            var body = preset.GetJson(prompt);
            var data = System.Text.Encoding.UTF8.GetBytes(body);
            var request = new UnityWebRequest(targetURL, "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", _auth);
            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();
            var json = request.downloadHandler.text.ToJson();
            onComplete(json);
        }
    }
}