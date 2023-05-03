// State.cs

using TMPro;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.Helpers
{
    public abstract class State<TOwner> : MonoBehaviour
    {
        public TOwner MyOwner { get; set; }
        public abstract void OnEnterState(TOwner stateManager, IPassableData rawData = null);
        public abstract void OnUpdateState(TOwner stateManager, IPassableData rawData = null);
        public abstract void OnExitState(TOwner stateManager, IPassableData rawData = null);
        public void ShowName(TextMeshProUGUI debugText) => debugText.text = GetType().Name;
    }
}