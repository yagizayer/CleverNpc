// NpcAnswerData.cs

using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class NpcAnswerData : IPassableData
    {
        public float BehaviourScore { get; set; }
        public string Answer { get; set; }
        public ConversationData ConversationData { get; set; }
    }
}