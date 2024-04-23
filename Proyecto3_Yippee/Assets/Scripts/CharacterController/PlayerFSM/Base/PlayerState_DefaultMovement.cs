using UnityEngine.InputSystem;

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
            throw new System.NotImplementedException();
        }

        public override void OnPlayerStay(InputValue inputs)
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}