// PlayerManager.cs

using System;
using System.Collections.Generic;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc;

namespace YagizAyer.Root.Scripts.Player
{
    public class PlayerManager : StateManager<PlayerManager>
    {
        public List<NpcManager> InteractableNpcs { get; } = new();

        #region Testing area

        [TextArea]
        [SerializeField]
        private string testPlayerPrompt;

        [ContextMenu("Test Conversation Prompt")]
        private void TestConversationPrompt() => Channels.ConversationPrompt.Raise(testPlayerPrompt.ToPassableData());

        #endregion


        private void Start() => SetState<States.Idle>();

        private void Update() => CurrentState.OnUpdateState(this);

        public void OnMovementInput(IPassableData rawData) => SetState<States.Move>(rawData);

        public void OnInteractionInput(IPassableData rawData)
        {
            if (InteractableNpcs.Count == 0) return;
            if (CurrentState is not States.Conversation)
                Channels.ConversationStart.Raise(new ConversationData
                {
                    NpcManager = transform.GetClosest(InteractableNpcs),
                    PlayerManager = this
                });
        }

        public void OnNpcEnterRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.SetState<Npc.States.PlayerInRange>(transform.ToPassableData());
            InteractableNpcs.Add(npc);
        }

        public void OnNpcExitRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.SetState<Npc.States.Idle>(transform.ToPassableData());
            InteractableNpcs.Remove(npc);
        }

        public void OnConversationStart(IPassableData rawData) => SetState<States.Conversation>(rawData);

        public void OnConversationPrompt(IPassableData rawData)
        {
            if (CurrentState is not States.Conversation conversationState) return;
            if (!rawData.Validate(out PassableDataBase<string> data)) return;
            conversationState.OnConversationPrompt(data.Value);
        }
    }
}