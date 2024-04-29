using InputController;
using UtilsComplements;

namespace AvatarController.PlayerFSM
{
    /// <summary>
    /// FreeMove, the regular state of the player
    /// </summary>
    public class PlayerState_DefaultMovement : PlayerState
    {
        public override string Name => "Default Movement";

        public PlayerState_DefaultMovement(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            _playerController.UnBlockMovement();
            //If necessary change the playerMovementData
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            _playerController.OnMovement?.Invoke(inputs.MoveInput);
            _playerController.OnJump?.Invoke(inputs.JumpInput);
            _playerController.OnDive?.Invoke(inputs.CrounchDiveInput);
            _playerController.OnInteract?.Invoke(inputs.InteractInput);
            _playerController.OnGhostView?.Invoke(inputs.GhostViewInput);
            //OnSprint?.Invoke(inputs.SprintInput);
        }
    }

    public class PlayerState_Poltergeist : PlayerState
    {
        public override string Name => "Default Movement";
        private Verify<PlayerPoltergeist> _playerPoltergeist;

        public PlayerState_Poltergeist(PlayerController playerController) : base(playerController)
        {
            _playerPoltergeist = new(playerController.gameObject);

            if (_playerPoltergeist)
                _playerPoltergeist.Component.enabled = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _playerPoltergeist.Component.enabled = true;
        }

        public override void OnPlayerStay(InputValues inputs)
        {

        }

        public override void OnExit()
        {
            base.OnExit();
            _playerPoltergeist.Component.enabled = false;
        }
    }
}