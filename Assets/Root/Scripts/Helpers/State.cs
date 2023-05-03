// State.cs

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    public abstract class State<TOwner> : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The states below cannot interrupt this state")]
        private List<State<TOwner>> excludedInterrupters = new(); 

        public TOwner MyOwner { get; set; }
        public List<State<TOwner>> ExcludedInterrupters => excludedInterrupters;
        public abstract void OnEnterState(TOwner stateManager, IPassableData rawData = null);
        public abstract void OnUpdateState(TOwner stateManager, IPassableData rawData = null);
        public abstract void OnExitState(TOwner stateManager, IPassableData rawData = null);
        public void ShowName(TextMeshProUGUI debugText) => debugText.text = GetType().Name;
    }
}