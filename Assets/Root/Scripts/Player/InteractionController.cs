// InteractionController.cs

using System.Linq;
using UnityEngine;
using YagizAyer.Root.Scripts.Npc;

namespace YagizAyer.Root.Scripts.Player
{
    public class InteractionController : PlayerComponent
    {
        [SerializeField]
        private SphereCollider interactionTrigger;

        [Range(0, 10)]
        [SerializeField]
        private float interactionRange = 5f;

        protected override void OnEnable()
        {
            base.OnEnable();
            OnValidate();
        }

        private void OnValidate()
        {
            if (interactionTrigger == null) return;
            interactionTrigger.radius = interactionRange;
        }

        public void OnNpcEnterRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.OnPlayerEnterRange(PlayerManager.transform);
        }

        public void OnNpcExitRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.OnPlayerExitRange(PlayerManager.transform);
        }
    }
}