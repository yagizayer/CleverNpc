// PlayerManager.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc;

namespace YagizAyer.Root.Scripts.Player
{
    public class PlayerManager : StateManager<PlayerManager>
    {
        private void Start() => SetState<States.Idle>();

        private void Update() => CurrentState.OnUpdateState(this);

        public void OnMovementInput(IPassableData rawData)
        {
            if (CurrentState is States.Move moveState) moveState.OnUpdateState(this, rawData);
            else SetState<States.Move>(rawData);
        }

        public void OnNpcEnterRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.SetState<Npc.States.Move>(transform.ToPassableData());
        }

        public void OnNpcExitRange(Collider other)
        {
            if (!other.TryGetComponent(out NpcManager npc)) return;
            npc.SetState<Npc.States.Idle>(transform.ToPassableData());
        }
    }
}