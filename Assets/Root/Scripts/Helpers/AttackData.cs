// AttackData.cs

using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class AttackData<TAttacker> : IPassableData where TAttacker : class
    {
        public TAttacker Attacker { get; set; }
        public Transform Target { get; set; }
    }
}