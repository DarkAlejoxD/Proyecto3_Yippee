using InputController;
using UtilsComplements;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_Poltergeist : PlayerState
    {
        private enum PoltergeistStates
        {
            Selecting,
            Manipulating
        }
        public override string Name => "Default Movement";
        private PoltergeistStates _currentState;

        public PlayerState_Poltergeist(PlayerController playerController) : base(playerController)
        {
            _currentState = PoltergeistStates.Selecting;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _currentState = PoltergeistStates.Selecting;
        }

        public override void OnPlayerStay(InputValues inputs)
        {

        }

        public override void OnExit()
        {
            base.OnExit();
            //_playerPoltergeist.Component.enabled = false;
        }
    }
}