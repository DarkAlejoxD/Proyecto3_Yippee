using InputController;
using UnityEngine;
using UtilsComplements;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_OnDive : PlayerState
    {
        public override string Name => "OnDive";
        private Verify<Animator> _verifyAnimator;
        private CharacterController _characterController;
        private PlayerDive _playerDive;

        private PlayerStates _lastState;

        public PlayerState_OnDive(PlayerController playerController) : base(playerController)
        {
            _verifyAnimator = new(playerController.gameObject);
            _characterController = playerController.GetComponent<CharacterController>();
            _playerDive = playerController.GetComponent<PlayerDive>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _characterController.height = 1;
            _lastState = _playerController.LastState;

            if (!_verifyAnimator)
                return;
            Animator animator = _verifyAnimator.Component;

            animator.SetBool("Dive", true);
            animator.SetBool("Idle", false);
        }
        public override void OnPlayerStay(InputValues inputs)
        {
            //_playerController.OnMovement?.Invoke(inputs.MoveInput);
            //_playerController.OnJump?.Invoke(inputs.JumpInput);
            //_playerController.OnDive?.Invoke(inputs.CrounchDiveInput);
            //_playerController.OnInteract?.Invoke(inputs.InteractInput);
            //_playerController.OnGhostView?.Invoke(inputs.GhostViewInput); //??
            //OnSprint?.Invoke(inputs.SprintInput);

            _playerController.RequestChangeState(_lastState);
        }

        public override void OnExit()
        {
            base.OnExit();
            _characterController.height = 2;
            if (!_verifyAnimator)
                return;
            _verifyAnimator[0].SetBool("Dive", false);
            _verifyAnimator[0].SetBool("Idle", true);//test
        }

        public override bool CanAutoTransition()
        {
            return !_playerDive.IsDiving;
        }
    }
}