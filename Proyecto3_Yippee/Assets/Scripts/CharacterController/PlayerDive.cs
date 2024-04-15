using AvatarController;
using AvatarController.Data;
using UnityEngine;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController), typeof(CharacterController))]

    public class PlayerDive : MonoBehaviour
{
    #region Fields
        private PlayerController _playerController;
        private CharacterController _characterController;
        private PlayerMovement _playerMovement;
        private Animator _animator;
        
        private PlayerData Data => _playerController.DataContainer;


        private bool _isDiving;
        private bool _isGrounded;


        private Vector3 _velocity;
    #endregion

    #region Unity Logic

        private void OnEnable()
        {
            if (_playerController == null)
            {
                _playerController = GetComponent<PlayerController>();
            }

            //_playerController. += OnDive;
        }

        private void OnDisable()
        {
            //_playerController. -= OnDive;
        }

        private void Awake()
        {            
            _playerController = GetComponent<PlayerController>();
            _characterController = GetComponent<CharacterController>();
            _playerMovement = GetComponent<PlayerMovement>();
            _animator = GetComponent<Animator>();
        }

    private void Update()
    {
        //DEBUG
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            OnDive();
        }

        if (_isDiving)
            DivingMovement();

            CheckGrounded();
    }
    #endregion

    #region Static Methods

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

        private void OnDive()
        {
            if (_isDiving)
                return;

            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();

            _velocity = forward * Data.DefaultDiveValues.StartingSpeed;
            _playerMovement.enabled = false;
            _isDiving = true;

            //DEBUG
                _animator.SetBool("Dive", true);
                _animator.SetBool("Idle", false);
                _characterController.height = 1;
            //
        }

        private void DivingMovement()
        {
            float deceleration = _isGrounded ? Data.DefaultDiveValues.GroundDeceleration : Data.DefaultDiveValues.AirDeceleration;
            _velocity -= Time.deltaTime * deceleration * _velocity.normalized;

            if (_velocity.magnitude < 0.1f)
            {
                _playerMovement.enabled = true;
                _isDiving = false;

                //DEBUG
                    _animator.SetBool("Dive", false);
                    _animator.SetBool("Idle", true);
                    _characterController.height = 2;
                //
            }

            _characterController.Move(_velocity);
        }

        private void CheckGrounded()
        {
            _isGrounded = _characterController.isGrounded;
        }

    #endregion
    }
}
