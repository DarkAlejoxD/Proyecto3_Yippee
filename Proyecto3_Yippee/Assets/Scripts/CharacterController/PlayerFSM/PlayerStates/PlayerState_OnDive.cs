using InputController;
using UnityEngine;
using UtilsComplements;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_OnDive : PlayerState
    {
        public override string Name => "OnDive";
        private readonly Verify<Animator> _verifyAnimator;
        private readonly CharacterController _characterController;
        private readonly PlayerDive _playerDive;

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

            if (!_verifyAnimator)
                return;
            Animator animator = _verifyAnimator.Component;

            animator.SetBool("Dive", true);
            animator.SetBool("Idle", false);
        }
        public override void OnPlayerStay(InputValues inputs)
        {
            if (Data.Powers.HasGhostView)
                _playerController.OnGhostView?.Invoke(inputs.GhostViewInput);

            if (_characterController.isGrounded) //Maybe send a raycast?
                _playerController.RequestChangeState(PlayerStates.OnGround);

            else
                _playerController.RequestChangeState(PlayerStates.OnAir);
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