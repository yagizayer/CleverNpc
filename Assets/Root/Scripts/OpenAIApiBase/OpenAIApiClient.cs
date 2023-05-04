using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityWebRequestAwaiter;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;

namespace YagizAyer.Root.Scripts.OpenAIApiBase
{
    [CreateAssetMenu(fileName = "OpenAIApiClient", menuName = "OpenAIApiClient")]
    public class OpenAIApiClient : ScriptableObject
    {
        private static readonly string Auth =
            $"Bearer {Environment.GetEnvironmentVariable("OPENAI_API_KEY", EnvironmentVariableTarget.User)}";

        public static async void RequestJsonAsync(string input, RequestPreset preset, Action<string> onComplete)
        {
            var body = preset.GetJson(input);
            var data = System.Text.Encoding.UTF8.GetBytes(body);
            var request = new UnityWebRequest(preset.TargetURL, "POST");

            request.SetRequestHeader("Content-Type", preset.BodyContent.ToContentString());
            request.SetRequestHeader("Authorization", Auth);

            request.uploadHandler = new UploadHandlerRaw(data);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest();
            var json = request.downloadHandler.text.ToJson();
            onComplete(json);
        }

        public static async void RequestFormAsync(RequestPreset preset, Action<string> onComplete)
        {
            if (preset.GetType().GetInterface(nameof(IWWWFormDataWrapper)) == null)
                throw new Exception("Preset must implement IWWWFormDataWrapper interface");

            var formDataWrapper = (IWWWFormDataWrapper)preset;
            var formData = new WWWForm();

            foreach (var kvPair in formDataWrapper.Fields) formData.AddField(kvPair.Key, kvPair.Value);
            foreach (var kvPair in formDataWrapper.FilePaths)
                formData.AddBinaryData(kvPair.Key, File.ReadAllBytes(kvPair.Value.filePath), kvPair.Value.fieldName,
                    kvPair.Value.mimeType);

            var request = UnityWebRequest.Post(preset.TargetURL, formData);
            request.SetRequestHeader("Authorization", Auth);


            await request.SendWebRequest();
            var json = request.downloadHandler.text.ToJson();
            onComplete(json);
        }


#if UNITY_EDITOR
        [MenuItem("OpenAI/API Test/Completion API")]
        public static void TestCompletionAPI()
        {
            const string input = "Testing the API in 25 tokens.";
            var preset = CreateInstance<CompletionPreset>();
            preset.maxTokens = 25;
            Action<string> onComplete = Debug.Log;

            RequestJsonAsync(input, preset, onComplete);
        }

        [MenuItem("OpenAI/API Test/Image API")]
        public static void TestImageAPI()
        {
            const string input = "A painting of a forest";
            var preset = CreateInstance<ImagePreset>();
            Action<string> onComplete = Debug.Log;

            RequestJsonAsync(input, preset, onComplete);
        }

        [MenuItem("OpenAI/API Test/Audio API")]
        public static void TestAudioAPI()
        {
            var preset = CreateInstance<AudioPreset>();
            Action<string> onComplete = Debug.Log;

            preset.Fields.Add("model", "whisper-1");
            preset.FilePaths.Add("file", new WWWFormData
            {
                fieldName = "TestSound.m4a",
                filePath = Application.dataPath + "/Resources/Test/TestSound.m4a",
                mimeType = "audio/m4a"
            });

            RequestFormAsync(preset, onComplete);
        }
#endif
    }
}