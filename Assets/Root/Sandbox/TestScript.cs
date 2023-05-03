using UnityEngine;
using YagizAyer.Root.Scripts.ApiBase;

namespace YagizAyer.Root.Sandbox
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField]
        private ApiClient apiClient;

        [SerializeField]
        private RequestPreset requestPreset;

        private string _prompt =
            "User: You are an expert sentiment analyst, you are analyzing 2 vectors : \n1- positivity : this is intention alignment of sentence. if sentence is perfectly positive, this will be 1 if sentence is perfectly negative, this will be -1\n2- friendliness : this is psychology alingment of sentence. if listener feels perfectly friendly after hearing the sentence, this will be 1,  if listener feels perfectly Hostile after hearing the sentence, this will be -1.\nyou can answer like this : \"P:0.32,F:-0.25\"\nsome examples : \n\t|Friendliness (-1)\t|Friendliness (0)\t|Friendliness (1)\nPositivity (-1)\t|\"I hate you\"\t|\"I am busy\"\t|\"I am sorry\"\nPositivity (0)\t|\"Leave me be\"\t|\"Nice weather today\"\t|\"What's up?\"\nPositivity (1)\t|\"I love you\"\t|\"Thank you so much\"\t|\"You're the best\"\nyour first sentence to analyze : \"I like hamburgers\"";

        [ContextMenu("Request")]
        public void Request() => apiClient.RequestAsync(_prompt, requestPreset, OnComplete);

        private void OnComplete(string json) => Debug.Log(json);
    }
}