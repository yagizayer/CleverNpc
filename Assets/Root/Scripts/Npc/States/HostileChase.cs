// HostileChase.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class HostileChase : Move
    {
        [SerializeField]
        private ParticleSystem chaseEffect;

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out NpcAnswerData data)) return;
            chaseEffect.Play();
            Channels.CancelConversating.Raise(data.ConversationData);
            base.OnEnterState(stateManager, data.ConversationData.PlayerManager.transform.ToPassableData());
        }

        protected override void OnReachTarget(Transform target) =>
            MyOwner.PlayAnimation(Animations.Attack.ToAnimationHash());
    }
}