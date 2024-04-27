using UnityEngine;
using AvatarController;
using AvatarController.Data;
using AvatarController.PlayerFSM;
using static UtilsComplements.AsyncTimer;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController), typeof(CharacterController))]
    public class PlayerDive : MonoBehaviour
    {
        #region Fields
        private PlayerController _playerController;
        private CharacterController _characterController;

        private Vector3 _velocity;

        public bool IsDiving { get; private set; }
        private bool _isGrounded;
        private bool _canDive;

        private PlayerData Data => _playerController.DataContainer;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            _characterController = GetComponent<CharacterController>();

            _canDive = true;
        }

        private void OnEnable()
        {
            if (_playerController == null)
                _playerController = GetComponent<PlayerController>();

            _playerController.OnDive += OnDive;
        }

        private void OnDisable()
        {
            _playerController.OnDive -= OnDive;
        }

        private void Update()
        {
            if (IsDiving)
            {
                DivingMovement();
                CheckGrounded();
            }
        }
        #endregion

        #region Private Methods

        private void OnDive(bool active)
        {
            if (!_canDive)
                return;

            if (!active) return;

            if (IsDiving)
                return;

            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            _velocity = forward * Data.DefaultDiveValues.StartingSpeed;
            IsDiving = true;

            _playerController.StopVelocity();
            _playerController.ForceChangeState(PlayerStates.OnDive);
            _canDive = false;
            StartCoroutine(TimerCoroutine(Data.DefaultDiveValues.Cooldown, () =>
            {
                _canDive = true;
            }));
            //
        }

        private void DivingMovement()
        {
            float deceleration = _isGrounded ? Data.DefaultDiveValues.GroundDeceleration// * Time.deltaTime
                : Data.DefaultDiveValues.AirDeceleration;// * Time.deltaTime;

            _velocity -= Time.deltaTime * deceleration * _velocity.normalized;

            if (_velocity.magnitude < Data.DefaultDiveValues.MinSpeedThreshold)
            {
                IsDiving = false;
                Debug.Log("Reach");                
                //DEBUG //Moved to the PlayeState_OnDive.OnExit()
                //_animator.SetBool("Dive", false);
                //_animator.SetBool("Idle", true);
                //
            }

            Vector3 motion = _velocity * Time.deltaTime * Data.DefOtherValues.ScaleMultiplicator;
            _characterController.Move(motion);
        }

        private void CheckGrounded() => _isGrounded = _characterController.isGrounded;

        #endregion
    }
}
