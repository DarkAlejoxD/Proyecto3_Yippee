﻿using UnityEngine;
using InputController;
using Unity.VisualScripting;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_Jumping : PlayerState
    {
        private readonly PlayerJump _playerJump;
        private float _timeToPeak;
        private float _timeWhenJumpStarted;
        private bool _isJumping;

        public override string Name => "Jumping";

        public PlayerState_Jumping(PlayerController playerController) : base(playerController)
        {
            if (!playerController.TryGetComponent(out _playerJump))
            {
                _playerJump = playerController.AddComponent<PlayerJump>();
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _timeToPeak = _playerJump.GetTimeToPeak();
            _timeWhenJumpStarted = Time.time;
            _isJumping = true;
            Debug.Log("Enteres");
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            _playerController.OnMovement?.Invoke(inputs.MoveInput);

            if (!_isJumping)
                return;

            if (!inputs.JumpInput)
            {
                _isJumping = false;
                float timePassed = Time.time - _timeWhenJumpStarted;
                float remainingTime = _timeToPeak - timePassed;

                remainingTime = Mathf.Clamp(remainingTime, 0, Data.DefaultJumpValues.ReleasePenalty);

                float Deceleration = (0 - _playerController.VelocityY) / remainingTime;

                _playerController.TwistGravity = Deceleration;
                _playerController.UseTwikedGravity = true;
                _playerController.ForceChangeState(PlayerStates.OnAir);
            }

            if (_timeWhenJumpStarted + _timeToPeak < Time.time)
            {
                _isJumping = false;
                _playerController.ForceChangeState(PlayerStates.OnAir);
            }
        }
    }
}