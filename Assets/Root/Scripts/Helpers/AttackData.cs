// AttackData.cs

using UnityEngine;

namespace YagizAyer.Root.Scripts.Helpers
{
    public class AttackData<TAttacker> where TAttacker : class
    {
        public TAttacker Attacker { get;  set; }
        public Transform Target { get;  set; }
    }
}