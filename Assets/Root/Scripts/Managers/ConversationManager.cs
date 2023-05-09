// ConversationManager.cs

using System;
using System.Collections;
using System.Collections.Generic;
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

        [Range(0, 20)]
        [SerializeField]
        private float timeoutLimit = 10;

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

        private Dictionary<int, bool> _requestTracker = new();

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
        internal static void RequestPlayerAudioTranscription(string path)
        {
            OpenAIApiClient.RequestFormAsync(path, Instance.audioSettings,
                onComplete: json =>
                {
                    var audioTranscription = AudioResponseData.FromJson(json).Text;
                    var requestId = Time.time.GetHashCode();
                    Instance._requestTracker.Add(requestId, false);

                    Instance.StartCoroutine(Instance.TimeoutRoutine(Time.time.GetHashCode(), audioTranscription));
                    Instance.RequestTextScoring(requestId, audioTranscription);

                    Channels.PlayerAnswering.Raise(audioTranscription.ToPassableData());
                });
        }

        // transcription -> Npc Answer
        private void RequestTextScoring(int requestId, string prompt)
        {
            if (!_requestTracker.ContainsKey(requestId)) return; // timed out and removed
            var npc = _npc;
            var tempHistoryAppend = "\nPlayer: " + prompt + "\nNpc: ";
            var answeringPrompt = npc.AnsweringInstructions + npc.chatHistory + tempHistoryAppend;


            OpenAIApiClient.RequestJsonAsync(answeringPrompt, completionSettings, onComplete: response =>
            {
                var fullAnswer = CompletionResponseData.FromJson(response).Choices[0].Text;
                if (ValidateResponse(fullAnswer))
                {
                    npc.chatHistory += tempHistoryAppend;
                    ResponseCb(requestId, response);
                }
                else if (++_retryCount < retryLimit) RequestTextScoring(requestId, prompt); // try again
                else
                {
                    Debug.LogWarning(fullAnswer);
                    npc.chatHistory += tempHistoryAppend + npc.DefaultAnswer;
                    npcAudioSource.PlayOneShot(npc.DefaultAudioClip);
                    _requestTracker[requestId] = true;
                    var answerData = new NpcAnswerData
                    {
                        AudioClip = npc.DefaultAudioClip,
                        Action = PossibleNpcActions.Talk,
                        Answer = npc.DefaultAnswer,
                        Npc = npc
                    };

                    Channels.NpcAnswering.Raise(answerData);
                }
            });
        }

        private void ResponseCb(int requestId, string response)
        {
            if (response == null) return;
            if (!_requestTracker.ContainsKey(requestId)) return; // timed out and removed

            _retryCount = 0;
            var fullAnswer = CompletionResponseData.FromJson(response).Choices[0].Text;
            var splitAnswer = fullAnswer.Split('|');
            splitAnswer[0].Trim().ToNpcAction(out var action);
            var answer = splitAnswer[1].Trim();

            _npc.chatHistory += fullAnswer;
            elevenLabsAc.RequestAsync(answer, _npc.Voice, onComplete: clip =>
            {
                _requestTracker[requestId] = true;
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

        private IEnumerator TimeoutRoutine(int hash, string prompt)
        {
            yield return new WaitForSeconds(Time.time + timeoutLimit);
            if (_requestTracker[hash])
            {
                _requestTracker.Remove(hash);
                yield break; // no problem
            }

            _requestTracker.Remove(hash);

            // timed out
            _npc.chatHistory += "\nPlayer: " + prompt + "\nNpc: " + _npc.TimedOutAnswer;
            npcAudioSource.PlayOneShot(_npc.TimedOutAudioClip);

            var answerData = new NpcAnswerData
            {
                AudioClip = _npc.TimedOutAudioClip,
                Action = PossibleNpcActions.Talk,
                Answer = _npc.TimedOutAnswer,
                Npc = _npc
            };

            Channels.NpcAnswering.Raise(answerData);
            _requestTracker.Remove(hash);
        }
    }
}