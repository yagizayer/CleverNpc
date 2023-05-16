// Idle.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Npc.States
{
    public class Idle : State<NpcManager>
    {
        private const float TransitionDuration = .25f;
        private float _transitionTimer;

        public override void OnEnterState(NpcManager stateManager, IPassableData rawData = null)
        {
            if (MyOwner.GetAnimationFloat(Animations.Walk.ToAnimationHash()) > 0)
                _transitionTimer = 0;
        }

        public override void OnUpdateState(NpcManager stateManager, IPassableData rawData = null)
        {
            _transitionTimer += Time.deltaTime;
            var animationValue = 1 - Mathf.Clamp01(_transitionTimer / TransitionDuration);
            MyOwner.SetAnimationFloat(Animations.Walk.ToAnimationHash(), animationValue);
        }

        public override void OnExitState(NpcManager stateManager, IPassableData rawData = null)
        {
            // do nothing
        }
    }
}