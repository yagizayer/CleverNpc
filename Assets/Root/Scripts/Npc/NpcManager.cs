// NpcManager.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc.States;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : StateManager<NpcManager>
    {
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

        public void OnCancelConversating(IPassableData rawData)
        {
            if (!rawData.Validate(out ConversationData data)) return;
            if (data.NpcManager != this) return;
            if (CurrentState is Conversation) SetState<PlayerInRange>(data.PlayerManager.ToPassableData());
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

            GameManager.ExecuteDelayed(data.AudioClip.length - .3f, () =>
            {
                switch (data.Action)
                {
                    case PossibleNpcActions.Attack:
                        SetState<HostileChase>(rawData);
                        break;
                    case PossibleNpcActions.Follow:
                        SetState<FriendlyChase>(rawData);
                        break;
                    default:
                    case PossibleNpcActions.Idle:
                        SetState<Conversation>(data.ConversationData);
                        break;
                }
            });
        }
    }
}