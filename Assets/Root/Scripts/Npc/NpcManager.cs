// NpcManager.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc.States;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : StateManager<NpcManager>
    {
        [SerializeField]
        private Animator myAnimator;

        [field: SerializeField] public string DefaultAnswer { get; private set; }

        [field: SerializeField]
        [field: TextArea(10, 10)]
        public string AnsweringInstructions { get; private set; }

        [SerializeField]
        [TextArea(20, 10)]
        public string chatHistory;

        #region Unity Methods

        private void Start() => SetState<Idle>();

        private void Update() => CurrentState.OnUpdateState(this);

        #endregion

        #region Event methods

        public void RaiseNpcAttackingChannel()
        {
            if (CurrentState is not Attack attack) return;
            var attackData = new AttackData<NpcManager>
            {
                Attacker = this,
                Target = attack.Target
            };
            Channels.NpcAttacking.Raise(attackData.ToPassableData());
        }

        public void RaiseNpcAttackEndingChannel()
        {
            if (CurrentState is not Attack attack) return;
            var attackData = new AttackData<NpcManager>
            {
                Attacker = this,
                Target = attack.Target
            };
            attack.Chase();
            Channels.NpcAttackEnding.Raise(attackData.ToPassableData());
        }

        public void OnConversating(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<NpcManager> data)) return;
            if (data.Value != this) return;
            SetState<Conversation>(rawData);
        }

        public void OnCancelConversating(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<NpcManager> data)) return;
            if (data.Value != this) return;
            if (CurrentState is Conversation) SetState<PlayerInRange>(GameManager.Player.transform.ToPassableData());
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
            if (data.Npc != this) return;

            GameManager.ExecuteDelayed(data.AudioClip.length - .3f, () =>
            {
                switch (data.Action)
                {
                    case PossibleNpcActions.Attack:
                        SetState<HostileChase>(GameManager.Player.transform.ToPassableData());
                        break;
                    case PossibleNpcActions.Train:
                        SetState<Train>(GameManager.DummyTarget.transform.ToPassableData());
                        break;
                    case PossibleNpcActions.Follow:
                        SetState<FriendlyChase>(GameManager.Player.transform.ToPassableData());
                        break;
                    default:
                    case PossibleNpcActions.Idle:
                        SetState<Conversation>(GameManager.Player.transform.ToPassableData());
                        break;
                }
            });
        }

        #endregion

        internal void PlayAnimation(int animationHash) => myAnimator.Play(animationHash);
    }
}