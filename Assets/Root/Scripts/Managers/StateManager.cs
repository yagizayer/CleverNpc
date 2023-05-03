// StateManager.cs

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;
using YagizAyer.Root.Scripts.Helpers;

namespace YagizAyer.Root.Scripts.Managers
{
    public abstract class StateManager<TOwner> : MonoBehaviour where TOwner : StateManager<TOwner>
    {
        [SerializeField]
        protected TextMeshProUGUI debugText;

        public State<TOwner> CurrentState { get; private set; }

        private Dictionary<Type, State<TOwner>> StatesDict { get; set; } = new();

        public virtual void OnEnable()
        {
            var states = GetComponentsInChildren<State<TOwner>>();
            foreach (var state in states) StatesDict.Add(state.GetType(), state);
        }

        public TState GetState<TState>() where TState : State<TOwner> =>
            StatesDict.TryGetValue(typeof(TState), out var state) ? state as TState : null;

        public void SetState<TState>(IPassableData rawData = null)
        {
            if (StatesDict.TryGetValue(typeof(TState), out var newState)) SetState(newState, rawData);
        }

        public void SetState(State<TOwner> newState, IPassableData rawData = null)
        {
            if (CurrentState != null)
                CurrentState.OnExitState(this as TOwner, rawData);

            CurrentState = newState;

            CurrentState.MyOwner = this as TOwner;
            CurrentState.ShowName(debugText);
            CurrentState.OnEnterState(this as TOwner, rawData);
        }
    }
}