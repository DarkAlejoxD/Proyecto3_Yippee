using UnityEngine;
using AvatarController.Data;
using UnityEngine.Playables;
using UnityEngine.Windows;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController), typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        //TODO: Make this movement script
        //TODO: Get PlayerData from PlayerController or whereever
        //TODO: Movement by acceleration, See Playetmovement from atka
        //TODO: Linear deceleration or something when the player stops moving decelerates, do it as you want
        //TODO: Subscribe to OnMove del PlayerController
        #region Fields

        private Vector3 _velocity;


        private PlayerController _playerController;
        private CharacterController _characterController;
        private PlayerData Data => _playerController.DataContainer;

        private Camera CurrentCamera => Camera.main; //TODO: Change this
        
        #endregion

        #region Static Methods
        public static void StaticMethod()
        {
        }
        #endregion

        #region Unity Logic

        private void OnEnable()
        {
            if( _playerController == null)
            {
                _playerController = GetComponent<PlayerController>();
            }

            _playerController.OnMovement += OnMovement;
        }

        private void OnDisable()
        {
            _playerController.OnMovement -= OnMovement;
        }


        private void Awake()
        {
            Application.targetFrameRate = 20;
            _playerController = GetComponent<PlayerController>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _velocity = Vector3.zero;
        }

        private void Update()
        {
            Deceleration();
            FaceDirection();
        }
        #endregion

        #region Public Methods
        public void PublicMethod()
        {
        }
        #endregion

        #region Private Methods
        private void OnMovement(Vector2 moveInput)
        {
            Vector3 forward = CalculateForward();
            Vector3 right = CalculateRight();

            Vector3 movement = Vector3.zero;

            movement = right * moveInput.x;
            movement += forward * moveInput.y;

            if (moveInput.magnitude == 0)
            {
                movement = Vector3.zero;
            }

            movement.Normalize();
            AcceleratedMovement(movement);
        }

        private Vector3 CalculateForward()
        {
            Vector3 forward = CurrentCamera.transform.forward;
            forward.y = 0;
            forward.Normalize();
            return forward;
        }

        private Vector3 CalculateRight()
        {
            Vector3 right = CurrentCamera.transform.right;
            right.y = 0;
            right.Normalize();
            return right;
        }


        private void AcceleratedMovement(Vector3 movement)
        {
            Vector3 motion;

            if(_velocity.magnitude < Data.DefaultMovement.MaxSpeed)
            {
                _velocity += Time.deltaTime * Data.DefaultMovement.Acceleration * movement;
            }



            motion = Time.deltaTime * _velocity;

            if (_velocity.magnitude < Data.DefaultMovement.MinSpeedToMove)
            {
                motion = Vector3.zero;
            }
            _characterController.Move(motion);
        }

        private void Deceleration()
        {
            if (_velocity.magnitude > 0)
                _velocity -= Time.deltaTime * Data.DefaultMovement.LinearDecceleration * (_velocity.normalized);
        }


        private void FaceDirection()
        {
            if (_velocity.magnitude > Data.DefaultMovement.MinSpeedToMove)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(_velocity.normalized);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Data.DefaultMovement.RotationLerp);
            }
        }
        #endregion
    }
}

