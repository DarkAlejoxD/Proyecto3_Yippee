using UnityEngine;
using AvatarController.LedgeGrabbing;
using InputController;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_Grabbing : PlayerState
    {
        [Range(0, 1)] private const float MIN_DOT_TO_LET_GO = 0.5f;
        public override string Name => "OnGrab";
        public PlayerState_Grabbing(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            //Maybe change the speed or smtg
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            _playerController.OnMovement?.Invoke(inputs.MoveInput);
            _playerController.OnJump?.Invoke(inputs.JumpInput);
            if (Data.Powers.HasGhostView)
                _playerController.OnGhostView?.Invoke(inputs.GhostViewInput);

            //float dot = Vector2.Dot(inputs.MoveInput, Vector2.down);
            //if (dot > MIN_DOT_TO_LET_GO)
            //    _playerController.GetComponent<PlayerLedgeGrab>().LetGoLedge();
        }
    }
}