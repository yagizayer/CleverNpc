// NpcManager.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc.States;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : StateManager<NpcManager>
    {
        [SerializeField]
        private Vector2 acceptableBehaviourRange;

        [SerializeField]
        private float currentBehaviourScore;

        [field: SerializeField]
        [field: TextArea(10, 10)]
        public string AnsweringInstructions { get; private set; }

        [SerializeField]
        [TextArea(20, 10)]
        public string chatHistory;

        private void Start() => SetState<Idle>();

        private void Update() => CurrentState.OnUpdateState(this);

        public void OnConversating(IPassableData rawData)
        {
            if (!rawData.Validate(out ConversationData data)) return;
            if (data.NpcManager != this) return;
            SetState<Conversation>(rawData);
        }

        public void OnNpcThinking(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<NpcManager> data)) return;
            if (data.Value != this) return;
            // do nothing
        }

        public void OnNpcAnswering(IPassableData rawData)
        {
            if (!rawData.Validate(out NpcAnswerData data)) return;

            currentBehaviourScore += data.BehaviourScore;

            if (currentBehaviourScore < acceptableBehaviourRange.x) SetState<HostileChase>();
            if (currentBehaviourScore > acceptableBehaviourRange.y) SetState<FriendlyChase>();
        }
    }
}