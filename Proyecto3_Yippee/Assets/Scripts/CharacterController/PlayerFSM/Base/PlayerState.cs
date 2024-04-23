using FSM;
using UnityEngine.InputSystem;

namespace AvatarController.PlayerFSM
{
    /// <summary>
    /// Call ReadInputs before OnStay
    /// </summary>
    public abstract class PlayerState : IState
    {
        public InputValue _inputs;
        protected PlayerController _playerController;

        public abstract string Name { get; }

        public PlayerState(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public virtual bool CanTransition() => true;
        public abstract void OnEnter();
        public void ReadInputs(InputValue inputs) => _inputs = inputs;
        public void OnStay() => OnPlayerStay(_inputs);
        public abstract void OnPlayerStay(InputValue inputs);
        public abstract void OnExit();
    }
}