using FSM;

namespace AvatarController.PlayerFSM
{
    public abstract class PlayerState : IState
    {
        public virtual bool CanTransition() => true;

        public abstract void OnEnter();

        public abstract void OnExit();

        public void OnStay()
        {
            throw new System.NotImplementedException();
        }
    }
}