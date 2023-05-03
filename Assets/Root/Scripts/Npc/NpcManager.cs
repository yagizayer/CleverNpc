// NpcManager.cs

using System;
using UnityEngine;
using System.Collections.Generic;
using YagizAyer.Root.Scripts.Helpers;
using YagizAyer.Root.Scripts.Managers;
using YagizAyer.Root.Scripts.Npc.States;

namespace YagizAyer.Root.Scripts.Npc
{
    public class NpcManager : StateManager<NpcManager>
    {
        private void Start() => SetState<Idle>();

        private void Update() => CurrentState.OnUpdateState(this);
    }
}