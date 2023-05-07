// Conversation.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Conversation : Idle
    {
        private AudioClip _recording;
        private ConversationData _conversationData;

        public ConversationData ConversationData => _conversationData;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out _conversationData)) return;
            base.OnEnterState(stateManager, _conversationData.NpcManager.ToPassableData()); // for NpcInRange.cs
        }

        internal void StartRecording()
        {
            _recording = Microphone.Start(null, false, 10, 44100);
            if (_recording == null) return;
            Channels.Recording.Raise(null);

            Debug.Log("Recording started");
        }

        internal void StopRecording()
        {
            Debug.Log("Recording stopped");
            if (Microphone.IsRecording(null)) Microphone.End(null);
            else Debug.LogError("Microphone is not recording");

            var path = _recording.Trim().SaveAsWav("Assets/Root/Audios/PlayerInput.wav");

            Channels.NpcThinking.Raise(_conversationData.NpcManager.ToPassableData());
            ConversationManager.RequestPlayerAudioTranscription(path);
            _recording = null;
        }

    }
}