using JetBrains.Annotations;
using System;

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
}
