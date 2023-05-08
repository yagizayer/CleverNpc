// DummyTarget.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Npc;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class DummyTarget : MonoBehaviour
    {
        [SerializeField]
        private Animator myAnimator;

        public void OnNpcAttacking(IPassableData rawData)
        {
            if (!rawData.Validate(out AttackData<NpcManager> data)) return;

            myAnimator ??= GetComponentInChildren<Animator>();
            if (data.Target == transform) myAnimator.Play(Animations.TakeDamage.ToAnimationHash());
        }
    }
}