// Attack.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.Base;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using Random = UnityEngine.Random;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Attack : State<NpcManager>
    {
        [SerializeField]
        private ParticleSystem attackEffect;

        [SerializeField]
        private AudioSource attackSound;
        [SerializeField]
        private AudioSource attackMissedSound;

        [Range(0, 2)]
        [SerializeField]
        private float attackDelay = .5f;

        public Transform Target { get; private set; }

        private Quaternion _targetRotation;
        private const float RotationSpeed = 5f;

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (!rawData.Validate(out PassableDataBase<Transform> data)) return;
            Target = data.Value;
            
            stateManager.SetAnimationTrigger(Animations.Attack.ToAnimationHash());
            
            GameManager.ExecuteDelayed(attackDelay, () => { attackEffect.Play(); });
                PlaySoundEffect(Vector3.Distance(MyOwner.transform.position, Target.position) < 2f);
            
            _targetRotation = Quaternion.LookRotation(Target.position - MyOwner.transform.position);
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            // called at every frame
            if (Target == null) return;
            MyOwner.transform.rotation = Quaternion.Slerp(MyOwner.transform.rotation, _targetRotation,
                RotationSpeed * Time.deltaTime);
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public void Chase() => MyOwner.SetState<HostileChase>(Target.ToPassableData());

        private void PlaySoundEffect(bool hit)
        {
            var targetSource = hit? attackSound : attackMissedSound;
            targetSource.pitch = Mathf.Clamp(attackSound.pitch + Random.value * .1f - .05f, .8f, 1.2f);
            targetSource.volume = Mathf.Clamp(attackSound.volume + Random.value * .1f - .05f, .3f, .7f);
            targetSource.Play();
        }
    }
}