using InputController;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_OnAir : PlayerState
    {
        private const string ANIM_BOOL_ONAIR = "OnAir";
        public override string Name => "OnAir";

        public PlayerState_OnAir(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (Anim)
            {
                Anim.SetBool(ANIM_BOOL_ONAIR, true);
            }
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            _playerController.OnMovement?.Invoke(inputs.MoveInput);
            _playerController.OnDive?.Invoke(inputs.CrounchDiveInput);

            if (Data.Powers.HasGhostView)
                _playerController.OnGhostView?.Invoke(inputs.GhostViewInput);
        }

        public override void OnExit()
        {
            base.OnExit();

            if (Anim)
            {
                Anim.SetBool(ANIM_BOOL_ONAIR, false);
            }
        }
    }
}