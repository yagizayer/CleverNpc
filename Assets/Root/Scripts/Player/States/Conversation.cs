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
        [SerializeField]
        private ConversationStates state = ConversationStates.Null;

        private AudioClip _recording;
        private ConversationData _conversationData;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out _conversationData)) return;
            base.OnEnterState(stateManager, _conversationData.NpcManager.ToPassableData()); // for NpcInRange.cs
            state = ConversationStates.Idle;
        }

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

            var path = _recording.Trim().SaveAsWav("Assets/Root/Audios/PlayerInput.wav");

            Channels.NpcThinking.Raise(_conversationData.NpcManager.ToPassableData());
            state = ConversationStates.Process;
            ConversationManager.RequestPlayerAudioTranscription(path);
            _recording = null;
        }

        internal void OnNpcAnswering()
        {
            if (state != ConversationStates.Process) return;
            state = ConversationStates.Idle;
        }

        private enum ConversationStates
        {
            Null = 0,
            Idle = 10,
            Record = 20,
            Process = 30,
        }
    }
}