using FSM;

namespace AvatarController.PlayerFSM
{
    public class FSM_Player : FSM_Base<PlayerStates, PlayerState>
    {
        public FSM_Player(PlayerStates rootKey, PlayerState rootState) : base(rootKey, rootState)
        {
        }

        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}