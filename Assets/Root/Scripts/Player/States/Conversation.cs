// Idle.cs

using System;
using System.Diagnostics;
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
    public class Conversation : State<PlayerManager>
    {
        [SerializeField]
        private OpenAIApiClient client;

        [SerializeField]
        private CompletionPreset completionSettings;

        [SerializeField]
        private AudioPreset audioSettings;

        [SerializeField]
        private ConversationStates state = ConversationStates.Null;

        private string _instructionPrompt;
        private const string Prefix = "\n\n\"";
        private const string Suffix = "\"\n\nAnswer:";
        private AudioClip _recording;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            _instructionPrompt ??= Resources.Load<TextAsset>("OpenAIApi/InstructionPrompt").text;
            state = ConversationStates.Idle;
        }

        public override void OnUpdateState(PlayerManager stateManager, IPassableData rawData = null)
        {
            // do nothing
        }

        public override void OnExitState(PlayerManager stateManager, IPassableData rawData = null)
        {
            // state = ConversationStates.Null;
        }

        internal void StartRecording()
        {
            if (state != ConversationStates.Idle) return;
            state = ConversationStates.Recording;

            _recording = Microphone.Start(null, false, 10, 44100);
            if (_recording == null) return;

            Debug.Log("Recording started");
        }

        internal void StopRecording()
        {
            if(state != ConversationStates.Recording) return;
            Debug.Log("Recording stopped");

            if (Microphone.IsRecording(null)) Microphone.End(null);
            else Debug.LogError("Microphone is not recording");

            var path = _recording.Trim().SaveAsWav();

            void OnAudioAPIResponse(string json)
            {
                var audioRP = AudioResponseData.FromJson(json);
                Channels.ConversationPrompt.Raise(audioRP.Text.ToPassableData());
            }

            state = ConversationStates.Processing;
            OpenAIApiClient.RequestFormAsync(path, audioSettings, OnAudioAPIResponse);
            _recording = null;
        }

        internal void OnConversationPrompt(string prompt)
        {
            if(state != ConversationStates.Processing) return;
            OpenAIApiClient.RequestJsonAsync(_instructionPrompt + Prefix + prompt + Suffix, completionSettings,
                OnAPIResponse);
        }

        private void OnAPIResponse(string response)
        {
            if(state != ConversationStates.Processing) return;
            if (response == null) return;

            state = ConversationStates.Idle;
            var responseData = CompletionResponseData.FromJson(response);
            var conversationResponseData = ConversationResponseData.FromJson(responseData.Choices[0].Text);
            Channels.ConversationResponse.Raise(conversationResponseData);
            Debug.Log($"Response: {conversationResponseData.positivity}, {conversationResponseData.friendliness}");
        }

        private enum ConversationStates
        {
            Null = 0,
            Idle = 10,
            Recording = 20,
            Processing = 30,
        }
    }
}