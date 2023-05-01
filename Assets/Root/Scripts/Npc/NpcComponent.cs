// NpcComponent.cs

using System;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Npc
{
    public abstract class NpcComponent : MonoBehaviour
    {
        protected NpcManager NpcManager;

        protected virtual void OnEnable() => NpcManager = GetComponentInParent<NpcManager>();

        protected virtual void OnDisable() => NpcManager = null;
    }
}