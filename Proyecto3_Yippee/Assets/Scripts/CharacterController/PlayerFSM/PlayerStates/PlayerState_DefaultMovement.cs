using InputController;
using UnityEngine;

namespace AvatarController.PlayerFSM
{
    /// <summary>
    /// FreeMove, the regular state of the player
    /// </summary>
    public class PlayerState_DefaultMovement : PlayerState
    {
        private const string MOVEMENT_VALUE = "Speed";
        private const string ONGROUND_ANIM = "OnGround";
        private const float SMOOTH = 0.2f;

        private float _animControl = 0;

        public override string Name => "Default Movement";
        private bool _poltergeistActivated;



        public PlayerState_DefaultMovement(PlayerController playerController) : base(playerController)
        {
            _poltergeistActivated = false;
        }

        public override void OnEnter()
        {
            _playerController.UnBlockMovement();
            //If necessary change the playerMovementData
            _poltergeistActivated = false;

            if (Anim)
                Anim.SetBool(ONGROUND_ANIM, true);
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            //Anim Logic
            if (Anim)
            {
                // 0----min(x)----max(y)
                float speed = _playerController.Velocity.magnitude;
                float minSpeed = Data.DefaultMovement.MinSpeedToMove;
                float maxSpeed = Data.DefaultMovement.MaxSpeed;
                float value;

                //min(0) ---- max(y-x)
                maxSpeed -= minSpeed;
                speed -= minSpeed;
                value = speed / maxSpeed;
                value = Mathf.Clamp01(value);
                _animControl = Mathf.Lerp(_animControl, value, SMOOTH);

                Anim.SetFloat(MOVEMENT_VALUE, _animControl);
            }

            //Inputs Logic
            _playerController.OnMovement?.Invoke(inputs.MoveInput);
            _playerController.OnJump?.Invoke(inputs.JumpInput);
            _playerController.OnDive?.Invoke(inputs.CrounchDiveInput);

            _playerController.OnInteract?.Invoke(inputs.InteractInput);

            if (Data.Powers.HasGhostView)
                _playerController.OnGhostView?.Invoke(inputs.GhostViewInput);

            if (!Data.Powers.HasPoltergeist)
                return;

            if (inputs.Poltergeist && !_poltergeistActivated &&
                _playerController._canActivatePoltergeist)
            {
                _poltergeistActivated = true;
                _playerController.RequestChangeState(PlayerStates.OnPoltergeist);
            }
            //_playerController.OnSprint?.Invoke(inputs.SprintInput); ???
        }

        public override void OnExit()
        {
            base.OnExit();

            if (Anim)
                Anim.SetBool(ONGROUND_ANIM, false);
        }
    }
}