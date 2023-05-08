// NpcAnswerData.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Npc;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class NpcAnswerData : IPassableData
    {
        public AudioClip AudioClip { get; set; }
        public PossibleNpcActions Action { get; set; }
        public string Answer { get; set; }
        public NpcManager Npc { get; set; }
    }
}