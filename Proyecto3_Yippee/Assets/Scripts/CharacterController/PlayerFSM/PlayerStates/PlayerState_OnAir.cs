using InputController;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_OnAir : PlayerState
    {
        public override string Name => "OnAir";

        public PlayerState_OnAir(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            _playerController.OnMovement?.Invoke(inputs.MoveInput);
            //_playerController.OnJump?.Invoke(inputs.JumpInput);
            _playerController.OnDive?.Invoke(inputs.CrounchDiveInput);
            //_playerController.OnInteract?.Invoke(inputs.InteractInput);
            _playerController.OnGhostView?.Invoke(inputs.GhostViewInput); //??
            //OnSprint?.Invoke(inputs.SprintInput);
        }
    }
}