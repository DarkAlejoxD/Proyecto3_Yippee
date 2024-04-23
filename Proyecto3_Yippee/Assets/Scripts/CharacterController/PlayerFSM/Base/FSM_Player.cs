using FSM;
using UnityEngine.InputSystem;

namespace AvatarController.PlayerFSM
{

    /// <summary>
    /// Use this as root because it's a bit different from the base FSM
    /// </summary>
    public class FSM_Player : FSM_Base<PlayerStates, PlayerState>
    {
        public FSM_Player() : base()
        {
        }

        public override void OnEnter()
        {
        }

        public void StayPlayer(InputValue inputs)
        {

        }

        public override void OnExit()
        {
        }

        public
    }
}