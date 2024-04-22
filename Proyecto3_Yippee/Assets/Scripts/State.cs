using JetBrains.Annotations;
using System;
using System.Collections.Generic;

namespace FSM
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnStay();

        bool CanTransition();
    }

    public class State : IState
    {
        public Action OnEnterDelegate;
        public Action OnStayDelegate;
        public Action OnExitDelegate;

        public State(Action onEnter, Action onStay, Action onExit)
        {
            OnEnterDelegate = onEnter;
            OnStayDelegate = onStay;
            OnExitDelegate = onExit;
        }

        public virtual bool CanTransition() => true;

        public void OnEnter() => OnEnterDelegate?.Invoke();

        public void OnExit() => OnExitDelegate?.Invoke();

        public void OnStay() => OnStayDelegate?.Invoke();
    }

    /// <summary>
    /// WIP
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class FSM_Base<TKey, TValue> : IState where TValue : IState
    {
        private Dictionary<TKey, TValue> _keyValuePairs;
        protected TValue _currentState;

        public FSM_Base()
        {
            _keyValuePairs = new Dictionary<TKey, TValue>();
            OnEnter();
        }

        public abstract void OnEnter();
        public void OnStay()
        {
            throw new NotImplementedException();
        }
        public abstract void OnExit();

        public void AddState(TKey key, TValue value)
        {
            if (!_keyValuePairs.ContainsKey(key))
            {
                _keyValuePairs.Add(key, value);
            }
            else
            {
                DEBUG_Warning("You are trying to add an existing key");
            }
        }

        public void ForceChange(TKey key)
        {
            if (_keyValuePairs.ContainsKey(key))
            {

            }
            else
            {
                DEBUG_Warning("This key doesn't exist, remain in the same state");
            }
        }

        protected static void DEBUG_Warning(string text)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(text);
#endif
        }

        public virtual bool CanTransition() => true;        
    }
}
