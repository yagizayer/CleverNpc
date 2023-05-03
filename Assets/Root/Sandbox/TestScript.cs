using UnityEngine;
using YagizAyer.Root.Scripts.OpenAIApiBase;

namespace YagizAyer.Root.Sandbox
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField]
        private OpenAIApiClient openAIApiClient;

        [SerializeField]
        private RequestPreset requestPreset;

        [SerializeField]
        [TextArea(3, 10)]
        private string prompt = "Say this is a test";

        [ContextMenu("Request")]
        public void Request() => openAIApiClient.RequestAsync(prompt, requestPreset, OnComplete);

        private void OnComplete(string json) => Debug.Log(json);
    }
}