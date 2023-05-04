// ConversationData.cs

using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Npc;
using YagizAyer.Root.Scripts.Player;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class ConversationData : IPassableData
    {
        public NpcManager NpcManager { get; set; }
        public PlayerManager PlayerManager { get; set; }
        public string Prompt { get; set; }
    }
}