using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// WIP
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="IState"></typeparam>
    public class FSM_Base<TKey, TValue> : IState where TValue : IState
    {
        private struct AutoTransition
        {
            public Transition TransitionTo;
            public TKey OtherState;

            public AutoTransition(Transition transition, TKey other)
            {
                TransitionTo = transition;
                OtherState = other;
            }
        }

        protected TKey _currentState;
        protected TKey _lastState;

        private Dictionary<TKey, TValue> _keyStatePair;
        private Dictionary<TKey, List<AutoTransition>> _autoTransitions;
        private bool _autoTransition = true;

        private readonly string NAME;

        public string Name => NAME;
        public IState CurrentState => _keyStatePair[_currentState];
        public IState LastState => _keyStatePair[_lastState];

        #region FSM Logic
        public FSM_Base(string name = "Default FSM")
        {
            NAME = name;
            _keyStatePair = new();
            _autoTransitions = new();
        }

        public FSM_Base(TKey rootKey, TValue rootState, string name = "Default FSM")
        {
            _keyStatePair = new();
            _autoTransitions = new();
            SetRoot(rootKey, rootState);
            NAME = name;
        }

        private IState this[TKey key] => _keyStatePair[key];

        public virtual bool CanTransition() => true;
        public virtual void OnEnter()
        { }
        public virtual void OnStay()
        {
            this[_currentState].OnStay();
            TransitionsUpdate();
        }

        public virtual void OnExit()
        { }
        #endregion

        #region Public Methods
        public void AddState(TKey key, TValue value)
        {
            if (!_keyStatePair.ContainsKey(key))
                _keyStatePair.Add(key, value);

            else
                DEBUG_Warning("You are trying to add an existing key");
        }

        public void AddAutoTransition(TKey from, Transition condiiton, TKey to)
        {
            if (!_keyStatePair.ContainsKey(to))
            {
                DEBUG_Warning("This state doesn't exists in the curren context, try add the state before adding the transition");
                return;
            }

            if (!_autoTransitions.ContainsKey(from))
            {
                if (!_keyStatePair.ContainsKey(from))
                {
                    DEBUG_Warning("This state doesn't exists in the curren context, try add the state before adding the transition");
                    return;
                }
                _autoTransitions.Add(from, new());
            }
            AutoTransition transition = new(condiiton, to);
            _autoTransitions[from].Add(transition);
        }

        public void SetRoot(TKey rootKey, TValue rootState)
        {
            if (_keyStatePair.ContainsKey(rootKey))
            {
                DEBUG_Warning("This key already exists");
                return;
            }
            _keyStatePair.Add(rootKey, rootState);
            _currentState = rootKey;
            this[_currentState].OnEnter();
        }

        public void ForceChange(TKey state)
        {
            if (_keyStatePair.ContainsKey(state))
                ChangeState(state);

            else
                DEBUG_Warning("This key doesn't exist, remain in the same state");
        }

        public void RequestChange(TKey state)
        {
            if (!_keyStatePair.ContainsKey(state))
            {
                DEBUG_Warning("This key doesn't exist, remain in the same state");
                return;
            }

            if (!this[_currentState].CanTransition())
                return;

            ChangeState(state);
        }
        #endregion

        protected virtual void ChangeState(TKey state)
        {
            if (state.Equals(_currentState))
            {
                DEBUG_Warning("Demanding change to the same State");
                return;
            }

            this[_currentState].OnExit();
            _lastState = _currentState;
            _currentState = state;
            this[_currentState].OnEnter();
            _autoTransition = true;
        }

        protected void TransitionsUpdate()
        {
            if (!_autoTransition)
                return;

            if (!_autoTransitions.ContainsKey(_currentState))
            {
                _autoTransition = false;
                return;
            }

            if (!this[_currentState].CanTransition())
                return;

            var list = _autoTransitions[_currentState];

            for (int i = 0; i < list.Count; i++)
            {
                AutoTransition trans = list[i];
                if (trans.TransitionTo.Condition())
                {
                    trans.TransitionTo.OnEndAction?.Invoke();
                    ForceChange(trans.OtherState);
                    break;
                }
            }
        }

        protected static void DEBUG_Warning(string text)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning(text);
#endif
        }
    }
}
