using UnityEngine;
using AvatarController.Data;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController), typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        private PlayerController _playerController;
        private CharacterController _characterController;

        private PlayerData Data => _playerController.DataContainer;
        private Vector3 Velocity
        {
            get => _playerController.Velocity;
            set => _playerController.Velocity = value;
        }
        private Camera CurrentCamera => Camera.main; //TODO: Change this //Don't need

        [Header("Some attributes")]
        private float _maxSpeed;

        private bool _grabbingLedge;
        private Vector3 _ledgeForward;
        #endregion        

        #region Unity Logic

        private void OnEnable()
        {
            if (_playerController == null)
                _playerController = GetComponent<PlayerController>();

            _playerController.OnMovement += OnMovement;
            _playerController.OnSprint += OnSprint;
            Velocity = Vector3.zero;
        }

        private void OnDisable()
        {
            _playerController.OnMovement -= OnMovement;
            _playerController.OnSprint -= OnSprint;
        }


        private void Awake()
        {
            Application.targetFrameRate = 60;
            _playerController = GetComponent<PlayerController>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _maxSpeed = Data.DefaultMovement.MaxSpeed;
        }

        private void Update()
        {
            Deceleration();
            FaceDirection();
        }
        #endregion

        #region Public Methods      
        public void FaceDirection(Vector3 dir)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Data.DefaultMovement.RotationLerp);
        }

        public void SetGrabbingLedgeMode(Vector3 ledgeForward)
        {
            _ledgeForward = ledgeForward;
            _grabbingLedge = true;
        }

        public void DisableGrabbingLedgeMode()
        {
            _grabbingLedge = false;
        }
        #endregion

        #region Private Methods
        private void OnMovement(Vector2 moveInput)
        {
            //if (_playerController.isPushing) return; //PROTO

            Vector3 forward = Vector3.zero;
            Vector3 right = Vector3.zero;

            if (!_grabbingLedge)
            {
                forward = CalculateForward();
                right = CalculateRight();
            }
            else
            {
                right = CalculateRight(_ledgeForward);
            }

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

        private Vector3 CalculateRight(Vector3 ledgeForward)
        {
            Vector3 right = Vector3.Cross(ledgeForward, transform.up);
            right.y = 0;
            right.Normalize();
            return right;
        }


        private void AcceleratedMovement(Vector3 movement)
        {
            Vector3 motion;

            float maxSpeed = _maxSpeed;
            if (_grabbingLedge) maxSpeed = Data.GrabbingLedgeMovement.MaxSpeed; //DEBUG

            if (Velocity.magnitude < maxSpeed)
                Velocity += Time.deltaTime * Data.DefaultMovement.Acceleration * movement;

            motion = Time.deltaTime * Velocity;

            if (Velocity.magnitude < Data.DefaultMovement.MinSpeedToMove)
                motion = Vector3.zero;
            
            _characterController.Move(motion * Data.DefOtherValues.ScaleMultiplicator);
        }

        private void Deceleration()
        {
            if (Velocity.magnitude > 0)
                Velocity -= Time.deltaTime * Data.DefaultMovement.LinearDecceleration * (Velocity.normalized);
        }

        private void FaceDirection()
        {
            if (_grabbingLedge) return;

            if (Velocity.magnitude > Data.DefaultMovement.MinSpeedToMove)
                FaceDirection(Velocity.normalized);
        }

        private void OnSprint(bool active)
        {
            if (active)
                _maxSpeed = Data.DefaultMovement.SprintMaxSpeed;

            else
                _maxSpeed = Data.DefaultMovement.MaxSpeed;
        }
        #endregion
    }
}

