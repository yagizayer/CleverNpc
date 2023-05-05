// ConversationManager.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;

namespace YagizAyer.Root.Scripts.Managers
{
    public class ConversationManager : SingletonBase<ConversationManager>
    {
        [SerializeField]
        private CompletionPreset completionSettings;

        [SerializeField]
        private AudioPreset audioSettings;

        [SerializeField]
        private TextAsset inputScoringPrompt;

        [SerializeField]
        private TextAsset answeringPrompt;

        // player audio file -> transcription
        internal static void RequestPlayerAudioTranscription(string path) =>
            OpenAIApiClient.RequestFormAsync(path, Instance.audioSettings,
                onComplete: json =>
                {
                    var audioTranscription = AudioResponseData.FromJson(json).Text;
                    Instance.RequestTextScoring(audioTranscription);
                });

        // transcription -> Text Score
        private void RequestTextScoring(string prompt) =>
            OpenAIApiClient.RequestJsonAsync(inputScoringPrompt.text + "\n\n\"" + prompt + "\"\n\nAnswer:",
                completionSettings,
                onComplete: response =>
                {
                    if (response == null) return;

                    var responseData = CompletionResponseData.FromJson(response);
                    var textScore = InputScore.FromJson(responseData.Choices[0].Text);

                    RequestNpcAnswer(prompt, textScore);
                });

        // Text Score + Prompt -> Npc Answer
        private void RequestNpcAnswer(string prompt, InputScore textScore)
        {
            var answerPrompt = answeringPrompt.text;
            var score = textScore.ToVector2();
            answerPrompt += "\n[P:" + score.x + ", F:" + score.y + "]"; // ex : [P:0.5, F:-0.5]
            answerPrompt += "\n\n\"" + prompt + "\"\n\nAnswer:";

            OpenAIApiClient.RequestJsonAsync(answerPrompt, completionSettings,
                onComplete: response => Channels.NpcAnswering.Raise(response.ToPassableData()));
        }
    }
}