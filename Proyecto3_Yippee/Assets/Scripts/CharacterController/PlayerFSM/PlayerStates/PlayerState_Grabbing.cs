using UnityEngine;
using InputController;
using static UtilsComplements.AsyncTimer;

namespace AvatarController.PlayerFSM
{
    public class PlayerState_Grabbing : PlayerState
    {
        //[Range(0, 1)] private const float MIN_DOT_TO_LET_GO = 0.5f;
        private const string GRAB_ANIM_TRIGGER = "Grabbing";
        private const string GRAB_ANIM_BOOL = "OnGrab";
        private const string MOVEMENT_VALUE = "GrabSpeed";
        private const float SMOOTH = 0.2f;

        private float _animControl = 0;
        public override string Name => "OnGrab";
        public PlayerState_Grabbing(PlayerController playerController) : base(playerController)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (Anim)
            {
                Anim.SetTrigger(GRAB_ANIM_TRIGGER);
                Anim.SetBool(GRAB_ANIM_BOOL, true);
            }
            _playerController._wasGrabbed = true;
            //_playerController.SetGravityActive(false);
            //_playerController.VelocityY = 0;
        }

        public override void OnPlayerStay(InputValues inputs)
        {
            if (Anim)
            {
                // 0----min(x)----max(y)
                float speed = _playerController.Velocity.magnitude;
                float minSpeed = Data.GrabbingLedgeMovement.MinSpeedToMove;
                float maxSpeed = Data.GrabbingLedgeMovement.MaxSpeed;
                float value;

                //min(0) ---- max(y-x)
                maxSpeed -= minSpeed;
                speed -= minSpeed;
                value = speed / maxSpeed;
                value = Mathf.Clamp01(value);
                _animControl = Mathf.Lerp(_animControl, value, SMOOTH);

                Anim.SetFloat(MOVEMENT_VALUE, _animControl);
            }

            _playerController.OnMovement?.Invoke(inputs.MoveInput);
            _playerController.OnJump?.Invoke(inputs.JumpInput);
            if (Data.Powers.HasGhostView)
                _playerController.OnGhostView?.Invoke(inputs.GhostViewInput);

            //float dot = Vector2.Dot(inputs.MoveInput, Vector2.down);
            //if (dot > MIN_DOT_TO_LET_GO)
            //    _playerController.GetComponent<PlayerLedgeGrab>().LetGoLedge();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (Anim)
                Anim.SetBool(GRAB_ANIM_BOOL, false);
            //_playerController.SetGravityActive(true);
        }
    }
}