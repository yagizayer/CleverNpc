// Idle.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Player.States
{
    public class Idle : State<PlayerManager>
    {

        public override void OnEnterState(PlayerManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public override void OnUpdateState(PlayerManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }

        public override void OnExitState(PlayerManager stateManager, IPassableData rawData = null)
        {
            // Do nothing
        }
    }
}