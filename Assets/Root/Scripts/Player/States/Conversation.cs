// Idle.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Conversation : State<PlayerManager>
    {
        [SerializeField]
        private OpenAIApiClient client;

        [SerializeField]
        private RequestPreset requestSettings;

        private string _instructionPrompt;

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            _instructionPrompt ??= Resources.Load<TextAsset>("InstructionPrompt").text;
        }

        public override void OnUpdateState(PlayerManager stateManager, IPassableData rawData = null)
        {
            // do nothing
        }

        public override void OnExitState(PlayerManager stateManager, IPassableData rawData = null)
        {
            // do nothing
        }

        internal void OnConversationPrompt(string prompt) =>
            client.RequestAsync(_instructionPrompt + prompt, requestSettings, OnResponse);

        private void OnResponse(string response)
        {
            if (response == null) return;
            var responseData = OpenAIResponseData.FromJson(response);
            var conversationResponseData = ConversationResponseData.FromJson(responseData.Choices[0].Text);
            Channels.ConversationResponse.Raise(conversationResponseData);
            Debug.Log($"Response: {conversationResponseData.positivity}, {conversationResponseData.friendliness}");
        }
    }
}