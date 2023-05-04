// Idle.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase;
using YagizAyer.Root.Scripts.OpenAIApiBase.Helpers;
using YagizAyer.Root.Scripts.OpenAIApiBase.Presets;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Conversation : State<PlayerManager>
    {
        [SerializeField]
        private OpenAIApiClient client;

        [SerializeField]
        private CompletionPreset completionSettings;

        private string _instructionPrompt;
        private const string Prefix = "\n\n\"";
        private const string Suffix = "\"\n\nAnswer:";

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            _instructionPrompt ??= Resources.Load<TextAsset>("OpenAIApi/InstructionPrompt").text;
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
            OpenAIApiClient.RequestJsonAsync(_instructionPrompt + Prefix + prompt + Suffix, completionSettings, OnResponse);

        private void OnResponse(string response)
        {
            if (response == null) return;
            var responseData = CompletionResponseData.FromJson(response);
            var conversationResponseData = ConversationResponseData.FromJson(responseData.Choices[0].Text);
            Channels.ConversationResponse.Raise(conversationResponseData);
            Debug.Log($"Response: {conversationResponseData.positivity}, {conversationResponseData.friendliness}");
        }
    }
}