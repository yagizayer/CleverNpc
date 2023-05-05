// Conversation.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;
using Debug = UnityEngine.Debug;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Conversation : Idle
    {

        [SerializeField]
        private OpenAIApiClient client;

        [SerializeField]
        private CompletionPreset completionSettings;

        [SerializeField]
        private AudioPreset audioSettings;

        [SerializeField]
        private ConversationStates state = ConversationStates.Null;

        [SerializeField]
        private TextAsset inputScoringPrompt;

        [SerializeField]
        private TextAsset answeringPrompt;

        private const string Prefix = "\n\n\"";
        private const string Suffix = "\"\n\nAnswer:";
        private AudioClip _recording;
        private ConversationData _conversationData;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out _conversationData)) return;
            base.OnEnterState(stateManager, _conversationData.NpcManager.ToPassableData()); // for NpcInRange.cs
            state = ConversationStates.Idle;
        }
        
        #region Record

        internal void StartRecording()
        {
            if (state != ConversationStates.Idle) return;
            state = ConversationStates.Record;

            _recording = Microphone.Start(null, false, 10, 44100);
            if (_recording == null) return;
            Channels.Recording.Raise(null);

            Debug.Log("Recording started");
        }

        internal void StopRecording()
        {
            if (state != ConversationStates.Record) return;
            Debug.Log("Recording stopped");

            if (Microphone.IsRecording(null)) Microphone.End(null);
            else Debug.LogError("Microphone is not recording");

            var path = _recording.Trim().SaveAsWav();


            Channels.NpcThinking.Raise(_conversationData.NpcManager.ToPassableData());
            state = ConversationStates.Process;
            RequestPlayerAudioTranscription(path);
            _recording = null;
        }

        #endregion

        #region API requests/responses

        private void RequestPlayerAudioTranscription(string path) =>
            OpenAIApiClient.RequestFormAsync(path, audioSettings, OnTranscriptionComplete);

        private void OnTranscriptionComplete(string json)
        {
            var audioTranscription = AudioResponseData.FromJson(json).Text;
            RequestTextScoring(audioTranscription);
        }

        private void RequestTextScoring(string prompt) =>
            OpenAIApiClient.RequestJsonAsync(
                inputScoringPrompt.text + Prefix + prompt + Suffix,
                completionSettings, response => OnScoringComplete(prompt, response));

        private void OnScoringComplete(string prompt, string response)
        {
            if (state != ConversationStates.Process) return;
            if (response == null) return;

            state = ConversationStates.Idle;
            var responseData = CompletionResponseData.FromJson(response);
            var textScore = InputScore.FromJson(responseData.Choices[0].Text);

            RequestNpcAnswer(prompt, textScore);
        }

        private void RequestNpcAnswer(string prompt, InputScore textScore)
        {
            var answerPrompt = answeringPrompt.text;
            var score = textScore.ToVector2();
            answerPrompt += "\n[P:" + score.x + ", F:" + score.y + "]"; // ex : [P:0.5, F:-0.5]
            answerPrompt += Prefix + prompt + Suffix;

            OpenAIApiClient.RequestJsonAsync(answerPrompt, completionSettings, OnAnswerComplete);
        }

        private void OnAnswerComplete(string response) => Debug.Log(response);

        #endregion

        private enum ConversationStates
        {
            Null = 0,
            Idle = 10,
            Record = 20,
            Process = 30,
        }
    }
}