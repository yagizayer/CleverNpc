// ConversationManager.cs

using UnityEngine;
using YagizAyer.Root.Scripts.ElevenLabsApiBase;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;

namespace YagizAyer.Root.Scripts.Managers
{
    public class ConversationManager : SingletonBase<ConversationManager>
    {
        [SerializeField]
        [Header("ElevenLabs Settings")]
        private ElevenLabsApiClient elevenLabsAc;

        [SerializeField]
        [Header("OpenAI Settings")]
        private CompletionPreset completionSettings;

        [SerializeField]
        private AudioPreset audioSettings;

        [SerializeField]
        [Header("References")]
        private AudioSource npcAudioSource;

        private static ConversationData _conversationData;
        public void OnConversating(IPassableData rawData) => rawData.Validate(out _conversationData);

        // player audio file -> transcription
        internal static void RequestPlayerAudioTranscription(string path) =>
            OpenAIApiClient.RequestFormAsync(path, Instance.audioSettings,
                onComplete: json =>
                {
                    var audioTranscription = AudioResponseData.FromJson(json).Text;
                    Debug.Log("prompt : " + audioTranscription);

                    _conversationData.NpcManager.chatHistory += "\nPlayer: " + audioTranscription + "\nNpc: ";
                    Instance.RequestTextScoring(audioTranscription);

                    Channels.PlayerAnswering.Raise(audioTranscription.ToPassableData());
                });

        // transcription -> Npc Answer
        private void RequestTextScoring(string prompt)
        {
            var answeringPrompt = _conversationData.NpcManager.AnsweringInstructions +
                                  _conversationData.NpcManager.chatHistory +
                                  prompt;

            OpenAIApiClient.RequestJsonAsync(answeringPrompt, completionSettings, onComplete: response =>
            {
                var fullAnswer = CompletionResponseData.FromJson(response).Choices[0].Text;
                Debug.LogWarning("answer : " + fullAnswer);
                if (ValidateResponse(fullAnswer)) ResponseCb(response);
                else RequestTextScoring(prompt); // try again
            });
        }

        private void ResponseCb(string response)
        {
            if (response == null) return;

            var fullAnswer = CompletionResponseData.FromJson(response).Choices[0].Text;
            var splitAnswer = fullAnswer.Split('|');
            var score = float.Parse(splitAnswer[0].Trim().Replace('.', ','));
            var answer = splitAnswer[1].Trim();

            _conversationData.NpcManager.chatHistory += fullAnswer;
            elevenLabsAc.RequestAsync(answer, onComplete: clip =>
            {
                npcAudioSource.PlayOneShot(clip);

                var answerData = new NpcAnswerData
                {
                    AudioClip = clip,
                    BehaviourScore = score,
                    Answer = answer,
                    ConversationData = _conversationData
                };

                Channels.NpcAnswering.Raise(answerData);
            });
        }

        private bool ValidateResponse(string response)
        {
            if (response == null) return false;
            var splitAnswer = response.Split('|');
            return splitAnswer.Length == 2 && 
                   float.TryParse(splitAnswer[0].Trim().Replace('.', ','), out _);
        }
    }
}