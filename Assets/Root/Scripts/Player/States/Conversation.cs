// Conversation.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Conversation : Idle
    {
        private AudioClip _recording;

        public NpcManager Npc { get; private set; }

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<NpcManager> data)) return;
            Npc = data.Value;
            base.OnEnterState(stateManager, Npc.ToPassableData()); // for NpcInRange.cs
        }

        internal void StartRecording()
        {
            _recording = Microphone.Start(null, false, 10, 44100);
            if (_recording == null) return;
            Channels.Recording.Raise(null);
        }

        internal void StopRecording()
        {
            if (Microphone.IsRecording(null)) Microphone.End(null);
            else Debug.LogError("Microphone is not recording");

            var path = _recording.Trim().SaveAsWav("Assets/Root/Audios/PlayerInput.wav");

            Channels.NpcThinking.Raise(Npc.ToPassableData());
            ConversationManager.RequestPlayerAudioTranscription(path);
            _recording = null;
        }

    }
}