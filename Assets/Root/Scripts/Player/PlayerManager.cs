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

        [ContextMenu("Test Prompting")]
        public void TestPrompting()
        {
            SetState<States.Conversation>(new ConversationData
            {
                NpcManager = transform.GetClosest(InteractableNpcs),
                PlayerManager = this,
                Prompt = "User: You are an expert sentiment analyst, you are analyzing 2 vectors : \n1- positivity : this is intention alignment of sentence. if sentence is perfectly positive, this will be 1 if sentence is perfectly negative, this will be -1\n2- friendliness : this is psychology alignment of sentence. if listener feels perfectly friendly after hearing the sentence, this will be 1,  if listener feels perfectly Hostile after hearing the sentence, this will be -1.\nyou can answer in JSON like this : {\"positivity\":\"0.32\",\"friendliness\":\"-0.25\"}\nsome examples : \n\t|Friendliness (-1)\t|Friendliness (0)\t|Friendliness (1)\nPositivity (-1)\t|\"I hate you\"\t|\"I am busy\"\t|\"I am sorry\"\nPositivity (0)\t|\"Leave me be\"\t|\"Nice weather today\"\t|\"What's up?\"\nPositivity (1)\t|\"I love you\"\t|\"Thank you so much\"\t|\"You're the best\"\n\n\"I dont like hamburgers as i dont like you.\"\nAnswer:"
            });
        }
    }
}