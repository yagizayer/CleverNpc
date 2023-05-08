// ConversationManager.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.ElevenLabsApiBase;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Npc;
using YagizAyer.Root.Scripts.OpenAIApiBase;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;

namespace YagizAyer.Root.Scripts.Managers
{
    public class ConversationManager : SingletonBase<ConversationManager>
    {
        [Range(0, 10)]
        [SerializeField]
        private int retryLimit = 2;

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

        [SerializeField]
        private PossibleNpcActions testAction = PossibleNpcActions.Null;

        private int _retryCount;

        private static NpcManager _npc;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (testAction == PossibleNpcActions.Null) return;
            if (!Application.isPlaying) return;

            Channels.NpcAnswering.Raise(new NpcAnswerData
            {
                Action = testAction,
                Answer = "Test Answer",
            });

            testAction = PossibleNpcActions.Null;
        }
#endif

        public void OnConversating(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<NpcManager> data)) return;
            _npc = data.Value;
        }

        // player audio file -> transcription
        internal static void RequestPlayerAudioTranscription(string path) =>
            OpenAIApiClient.RequestFormAsync(path, Instance.audioSettings,
                onComplete: json =>
                {
                    var audioTranscription = AudioResponseData.FromJson(json).Text;
                    Debug.Log("prompt : " + audioTranscription);
                    Instance.RequestTextScoring(audioTranscription);

                    Channels.PlayerAnswering.Raise(audioTranscription.ToPassableData());
                });

        // transcription -> Npc Answer
        private void RequestTextScoring(string prompt)
        {
            var npc = _npc;
            var tempHistoryAppend = "\nPlayer: " + prompt + "\nNpc: ";
            var answeringPrompt = npc.AnsweringInstructions + npc.chatHistory + tempHistoryAppend;
            OpenAIApiClient.RequestJsonAsync(answeringPrompt, completionSettings, onComplete: response =>
            {
                var fullAnswer = CompletionResponseData.FromJson(response).Choices[0].Text;
                Debug.LogWarning("answer : " + fullAnswer);
                if (ValidateResponse(fullAnswer))
                {
                    npc.chatHistory += tempHistoryAppend;
                    ResponseCb(response);
                }
                else if (++_retryCount < retryLimit) RequestTextScoring(prompt); // try again
                else
                {
                    Debug.LogWarning("Answer is not valid");
                    Debug.LogWarning("Default : " + npc.DefaultAnswer);
                    npc.chatHistory += tempHistoryAppend;
                    ResponseCb(npc.DefaultAnswer, true);
                }
            });
        }

        private void ResponseCb(string response, bool isDefault = false)
        {
            if (response == null) return;

            _retryCount = 0;
            var fullAnswer = isDefault ? response : CompletionResponseData.FromJson(response).Choices[0].Text;
            var splitAnswer = fullAnswer.Split('|');
            splitAnswer[0].Trim().ToNpcAction(out var action);
            var answer = splitAnswer[1].Trim();

            _npc.chatHistory += fullAnswer;
            elevenLabsAc.RequestAsync(answer, onComplete: clip =>
            {
                npcAudioSource.PlayOneShot(clip);

                var answerData = new NpcAnswerData
                {
                    AudioClip = clip,
                    Action = action,
                    Answer = answer,
                    Npc = _npc
                };

                Channels.NpcAnswering.Raise(answerData);
            });
        }

        private bool ValidateResponse(string response)
        {
            if (response == null) return false;
            var splitAnswer = response.Split('|');

            return splitAnswer.Length == 2 &&
                   splitAnswer[0].Trim().ToNpcAction(out _);
        }
    }
}