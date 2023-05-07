// PlayerManager.cs

using System.Collections.Generic;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc;
using YagizAyer.Root.Scripts.Player.States;

namespace YagizAyer.Root.Scripts.Player
{
    public class PlayerManager : StateManager<PlayerManager>
    {
        [SerializeField]
        private Animator myAnimator;

        public List<NpcManager> InteractableNpcs { get; } = new();

        #region UnityEvents

        private void Start() => SetState<Idle>();

        private void Update() => CurrentState.OnUpdateState(this);

        #endregion

        #region Inputs

        public void OnMovementInput(IPassableData rawData) => SetState<Move>(rawData);

        public void OnRecordingInput(IPassableData rawData)
        {
            if (CurrentState is not Conversation conversationState) return;
            if (!rawData.Validate(out PassableDataBase<bool> data)) return;

            if (data.Value) conversationState.StartRecording();
            else conversationState.StopRecording();
        }

        public void OnCancelInput(IPassableData _)
        {
            if (CurrentState is not Conversation conversation) return;
            Channels.CancelConversating.Raise(conversation.ConversationData);
        }

        #endregion

        #region Conversation

        public void OnInteractionInput(IPassableData rawData)
        {
            if (InteractableNpcs.Count == 0) return;

            var conversationData = new ConversationData
            {
                NpcManager = transform.GetClosest(InteractableNpcs),
                PlayerManager = this
            };
            Channels.Conversating.Raise(conversationData);
            SetState<Conversation>(conversationData);
        }

        public void OnCancelConversation(IPassableData _) => SetState<Idle>();

        #endregion

        #region Gameplay triggers

        public void OnNpcEnterRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.SetState<Npc.States.PlayerInRange>(this.ToPassableData());
            InteractableNpcs.Add(npc);
        }

        public void OnNpcExitRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.SetState<Npc.States.Idle>(this.ToPassableData());
            InteractableNpcs.Remove(npc);
        }

        #endregion

        internal void PlayAnimation(int animationHash) => myAnimator.Play(animationHash);
    }
}