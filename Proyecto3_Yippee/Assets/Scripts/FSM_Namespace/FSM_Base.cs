using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// WIP
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class FSM_Base<TKey, TValue> : IState where TValue : IState
    {
        private Dictionary<TKey, TValue> _keyValuePairs;
        protected TValue _currentState;
        protected TValue _lastState;

        #region FSM Logic
        public FSM_Base(TKey rootKey, TValue rootState)
        {
            OnEnter();
            _keyValuePairs = new Dictionary<TKey, TValue>();
            _keyValuePairs.Add(rootKey, rootState);
            _currentState = rootState;
            _currentState.OnEnter();
        }

        public virtual bool CanTransition() => true;
        public abstract void OnEnter();
        public virtual void OnStay() => _currentState.OnStay();
        public abstract void OnExit();
        #endregion

        #region Public Methods
        public void AddState(TKey key, TValue value)
        {
            if (!_keyValuePairs.ContainsKey(key))
                _keyValuePairs.Add(key, value);

            else
                DEBUG_Warning("You are trying to add an existing key");
        }

        public void ForceChange(TKey state)
        {
            if (_keyValuePairs.ContainsKey(state))
                ChangeState(state);

            else
                DEBUG_Warning("This key doesn't exist, remain in the same state");
        }

        #endregion

        protected virtual void ChangeState(TKey state)
        {
            _currentState.OnExit();
            _lastState = _currentState;
            _currentState = _keyValuePairs[state];
            _currentState.OnEnter();
        }

        protected static void DEBUG_Warning(string text)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(text);
#endif
        }
    }
}
