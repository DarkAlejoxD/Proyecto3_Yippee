using InputController;
using Poltergeist;
using static UtilsComplements.AsyncTimer;

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
        private bool _canChangeState = false;

        public PlayerState_Poltergeist(PlayerController playerController) : base(playerController)
        {
            _currentState = PoltergeistStates.Selecting;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _currentState = PoltergeistStates.Selecting;
            _canChangeState = false;
            _playerController.StartCoroutine(
                TimerCoroutine(_playerController.DataContainer.DefPoltValues.PoltergeistSpamCD,
                () => _canChangeState = true));
            _playerController.StartPoltergeist();
            _playerController.OnPoltergeistEnter?.Invoke();

            CameraPolter.ActivatePolterMode();
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            switch (_currentState)
            {
                case PoltergeistStates.Selecting:


                    break;
                case PoltergeistStates.Manipulating:


                    break;
            }

            if (inputs.InteractInput)
            {
                _playerController.RequestChangeState(PlayerStates.OnGround);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            CameraPolter.DeactivatePolterMode();
            _playerController.EndPoltergeist();
        }

        public override bool CanAutoTransition()
        {
            return _canChangeState;
        }
    }
}