using FSM;
using InputController;

namespace AvatarController.PlayerFSM
{
    /// <summary>
    /// Use this as root because it's a bit different from the base FSM.
    /// It uses FSM_Base as base
    /// </summary>
    public class FSM_Player : FSM_Base<PlayerStates, PlayerState>
    {
        //TODO: Test, and maybe write all the logic here?
        public void StayPlayer(InputValues inputs)
        {
            this[_currentState].OnPlayerStay(inputs);
            TransitionsUpdate();
        }
    }
}