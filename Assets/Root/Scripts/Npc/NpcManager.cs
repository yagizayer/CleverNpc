// NpcManager.cs

using System;
using UnityEngine;
using UnityEngine.AI;
using YagizAyer.Root.Scripts.ElevenLabsApiBase.Helpers;
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
        [Header("References")]
        private Animator listeningCountdown;

        [SerializeField]
        private Animator thinkingCountdown;

        [SerializeField]
        private Animator myAnimator;

        [SerializeField]
        private NavMeshAgent myAgent;

        [field: SerializeField]
        [field: Header("Limitations")]
        public string DefaultAnswer { get; private set; }

        [field: SerializeField] public AudioClip DefaultAudioClip { get; private set; }
        [field: SerializeField] public string TimedOutAnswer { get; private set; }
        [field: SerializeField] public AudioClip TimedOutAudioClip { get; private set; }

        [field: SerializeField] public Voices Voice { get; private set; }

        [field: SerializeField]
        [field: TextArea(10, 10)]
        public string AnsweringInstructions { get; private set; }

        [SerializeField]
        [TextArea(20, 10)]
        public string chatHistory;

        public NavMeshAgent Agent => myAgent;

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
            Channels.NpcAttacking.Raise(attackData);
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
            Channels.NpcAttackEnding.Raise(attackData);
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

        public void OnPlayerRecordInput(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<bool> data)) return;

            if (CurrentState is not Conversation) return;

            if (data.Value) listeningCountdown.Play(Animations.Show.ToAnimationHash());
            if (!data.Value) listeningCountdown.Play(Animations.Hide.ToAnimationHash());
        }

        public void OnNpcThinking(IPassableData rawData)
        {
            if (!rawData.Validate(out PassableDataBase<NpcManager> data)) return;
            if (CurrentState is not Conversation) return;
            if (data.Value != this) return;
            listeningCountdown.Play(Animations.Hide.ToAnimationHash());
            thinkingCountdown.Play(Animations.Show.ToAnimationHash());
        }

        public void OnNpcAnswering(IPassableData rawData)
        {
            if (!rawData.Validate(out NpcAnswerData data)) return;
            if (data.Npc != null && data.Npc != this) return; // if null it is for testing

            thinkingCountdown.Play(Animations.Hide.ToAnimationHash());
            var waitDuration = data.AudioClip != null ? data.AudioClip.length - .3f : .1f;

            GameManager.ExecuteDelayed(waitDuration, () =>
            {
                switch (data.Action)
                {
                    case PossibleNpcActions.Attack:
                        SetState<HostileChase>(GameManager.Player.transform.ToPassableData());
                        break;
                    case PossibleNpcActions.Train:
                        SetState<HostileChase>(GameManager.DummyTarget.transform.ToPassableData());
                        break;
                    case PossibleNpcActions.Follow:
                        SetState<FriendlyChase>(GameManager.Player.transform.ToPassableData());
                        break;
                    case PossibleNpcActions.Wait:
                        Channels.CancelConversating.Raise(this.ToPassableData());
                        SetState<Idle>();
                        break;
                    case PossibleNpcActions.Null:
                    case PossibleNpcActions.Talk:
                    default:
                        SetState<Conversation>(GameManager.Player.transform.ToPassableData());
                        break;
                }
            });
        }

        #endregion

        internal void SetAnimationFloat(int floatHash, float value) => myAnimator.SetFloat(floatHash, value);
        internal float GetAnimationFloat(int floatHash) => myAnimator.GetFloat(floatHash);
        internal void SetAnimationTrigger(int triggerHash) => myAnimator.SetTrigger(triggerHash);
    }
}