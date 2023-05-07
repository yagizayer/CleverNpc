// NpcAnswerData.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class NpcAnswerData : IPassableData
    {
        public AudioClip AudioClip { get; set; }
        public PossibleNpcActions Action { get; set; }
        public string Answer { get; set; }
        public ConversationData ConversationData { get; set; }
    }
}