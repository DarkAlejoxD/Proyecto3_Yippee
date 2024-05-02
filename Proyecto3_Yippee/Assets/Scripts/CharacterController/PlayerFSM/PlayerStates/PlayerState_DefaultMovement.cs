using InputController;
using UnityEngine;

namespace AvatarController.PlayerFSM
{
    /// <summary>
    /// FreeMove, the regular state of the player
    /// </summary>
    public class PlayerState_DefaultMovement : PlayerState
    {
        public override string Name => "Default Movement";
        private bool _poltergeistActivated;

        public PlayerState_DefaultMovement(PlayerController playerController) : base(playerController)
        {
            _poltergeistActivated = false;
        }

        public override void OnEnter()
        {
            _playerController.UnBlockMovement();
            //If necessary change the playerMovementData
            _poltergeistActivated = false;
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            _playerController.OnMovement?.Invoke(inputs.MoveInput);
            _playerController.OnJump?.Invoke(inputs.JumpInput);
            _playerController.OnDive?.Invoke(inputs.CrounchDiveInput);

            _playerController.OnInteract?.Invoke(inputs.InteractInput);
            _playerController.OnGhostView?.Invoke(inputs.GhostViewInput);

            if (inputs.Poltergeist && !_poltergeistActivated)
            {
                Debug.Log("AAAAAA");
                _poltergeistActivated = true;
                _playerController.RequestChangeState(PlayerStates.OnPoltergeist);
            }
            //_playerController.OnSprint?.Invoke(inputs.SprintInput); ???
        }
    }
}