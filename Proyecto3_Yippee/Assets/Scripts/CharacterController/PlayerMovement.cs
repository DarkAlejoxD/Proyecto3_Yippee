using UnityEngine;
using AvatarController.Data;

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


        private void Awake()
        {
            Application.targetFrameRate = 60;
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

            _characterController.Move(motion);
        }

        private void Deceleration()
        {
            _velocity -= Time.deltaTime * Data.DefaultMovement.LinearDecceleration * (_velocity.normalized);
        }
        #endregion
    }
}

