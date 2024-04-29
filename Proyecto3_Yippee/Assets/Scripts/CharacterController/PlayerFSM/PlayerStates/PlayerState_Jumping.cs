using InputController;
using UnityEngine;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_Jumping : PlayerState
    {
        public override string Name => "Jumping";

        public PlayerState_Jumping(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            Debug.Log("Not implemented Jumping");
        }
    }
}