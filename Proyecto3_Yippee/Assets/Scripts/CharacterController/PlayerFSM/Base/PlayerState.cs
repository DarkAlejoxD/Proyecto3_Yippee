using FSM;
using InputController;

namespace AvatarController.PlayerFSM
{
    /// <summary>
    /// Call ReadInputs before OnStay
    /// </summary>
    public abstract class PlayerState : IState
    {
        public InputValues _inputs;
        protected PlayerController _playerController;

        public abstract string Name { get; }

        public PlayerState(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public virtual bool CanAutoTransition() => true;
        public virtual void OnEnter() { }
        public void ReadInputs(InputValues inputs) => _inputs = inputs;
        public void OnStay() => OnPlayerStay(_inputs);
        public abstract void OnPlayerStay(InputValues inputs);
        public virtual void OnExit() { }
    }
}