// NpcManager.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc.States;

#if UNITY_EDITOR
using UnityEditor.ShortcutManagement;
#endif

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : StateManager<NpcManager>
    {
#if UNITY_EDITOR
        private static NpcManager _instance;

        public override void OnEnable()
        {
            base.OnEnable();
            _instance = this;
        }
#endif

        internal Vector2 BehaviouralOrientation;

        private void Start() => SetState<Idle>();

        private void Update() => CurrentState.OnUpdateState(this);

        public void OnConversationStart(IPassableData rawData) => SetState<Conversation>(rawData);

        public void OnConversationResponse(IPassableData rawData)
        {
            if(CurrentState is not Conversation conversationState) return;
            if (!rawData.Validate(out ConversationResponseData data)) return;
            conversationState.OnConversationResponse(data);
        }

#if UNITY_EDITOR

        // shortcuts for testing : 
        // shift + 1 : friendly chase
        // shift + 2 : hostile chase
        // shift + 3 : return chase
        // shift + 4 : go home

        [Shortcut("Custom Shortcuts/Friendly Chase", KeyCode.Alpha1, ShortcutModifiers.Shift)]
        private static void FriendlyChase() => _instance.SetState<FriendlyChase>();

        [Shortcut("Custom Shortcuts/Hostile Chase", KeyCode.Alpha2, ShortcutModifiers.Shift)]
        private static void HostileChase() => _instance.SetState<HostileChase>();

        [Shortcut("Custom Shortcuts/Return Chase", KeyCode.Alpha3, ShortcutModifiers.Shift)]
        private static void ReturnChase() => _instance.SetState<ReturnChase>();

        [Shortcut("Custom Shortcuts/Go Home", KeyCode.Alpha4, ShortcutModifiers.Shift)]
        private static void GoHome() => _instance.SetState<GoHome>();

#endif
    }
}