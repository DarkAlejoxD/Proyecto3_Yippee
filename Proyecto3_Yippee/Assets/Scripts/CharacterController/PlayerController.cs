using System;
using UnityEngine;
using AvatarController.Data;
using AvatarController.PlayerFSM;
using InputController;
using UtilsComplements;

namespace AvatarController
{
    [RequireComponent(typeof(InputManager), typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        #region Fields
        [Header("Data")]
        [SerializeField] private PlayerData _dataContainer;
        private CharacterController _characterController;

        [Header("Delegates")]
        public Action<Vector2> OnMovement;
        public Action<bool> OnJump;
        public Action<bool> OnDive;
        public Action<bool> OnInteract;
        public Action<bool> OnGhostView;
        public Action<bool> OnSprint;
        public Action OnPoltergeistEnter;
        public Action<Vector2, float> OnPoltergeistStay;
        public Action<bool> OnPoltergeistExit;

        [Header("Random Attributes")]
        public bool isPushing = false;
        private PlayerMovement _playerMovement;
        private PlayerJump _playerJump;
        private PlayerDive _playerDive;

        [Header("FSM")]
        private FSM_Player _playerFSM;
        #endregion

        public PlayerData DataContainer => _dataContainer;
        public bool IsGrounded => _playerJump.IsGrounded;

        #region Unity Logic
        private void Awake()
        {
            GameManager.GetGameManager().SetPlayerInstance(this);
            _playerMovement = GetComponent<PlayerMovement>();
            _playerJump = GetComponent<PlayerJump>();
            _characterController = GetComponent<CharacterController>();
            _playerDive = GetComponent<PlayerDive>();

            FSMInit();
        }        

        private void OnEnable()
        {
            if (!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected += OnGetInputs;
        }

        private void OnDisable()
        {
            if (!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected -= OnGetInputs;
        }
        #endregion

        #region Public Methods
        public void BlockMovement(Vector3 dir)
        {
            _characterController.enabled = false;
            _playerDive.enabled = false;
            _playerJump.enabled = false;

            Quaternion desiredRotation = Quaternion.LookRotation(dir);
            transform.rotation = desiredRotation;
            _playerMovement.StopVelocity();
            _playerMovement.enabled = false;
        }

        public void UnBlockMovement()
        {
            _characterController.enabled = true;
            _playerDive.enabled = true;
            _playerJump.enabled = true;
            _playerMovement.enabled = true;
            _playerMovement.StopVelocity();

        }

        public void RequestTeleport(Vector3 position)
        {
            BlockMovement(transform.forward);
            transform.position = position;
            UnBlockMovement();
        }

        #endregion

        #region Private Methods
        private void FSMInit()
        {
            _playerFSM = new();

            _playerFSM.SetRoot(PlayerStates.OnGround, new PlayerState_DefaultMovement(this));

            _playerFSM.OnEnter();
        }

        private void OnGetInputs(InputValues inputs) //TODO: Separate States
        {
            _playerFSM.StayPlayer(inputs);

            #region OLD
            //switch (_currentState)
            //{
            //    case PlayerStates.OnPoltergeist:
            //        {
            //            float upDown = 0;
            //            if (inputs.JumpInput) upDown += 1;
            //            if (inputs.CrounchDiveInput) upDown -= 1;
            //            OnPoltergeistStay?.Invoke(inputs.MoveInput, upDown);
            //            OnPoltergeistExit?.Invoke(inputs.CancelInput);
            //        }
            //        break;
            //    case PlayerStates.OnDive:
            //        break;
            //    default:
            //        {
            //            OnMovement?.Invoke(inputs.MoveInput);
            //            OnJump?.Invoke(inputs.JumpInput);
            //            OnDive?.Invoke(inputs.CrounchDiveInput);
            //            OnInteract?.Invoke(inputs.InteractInput);
            //            OnGhostView?.Invoke(inputs.GhostViewInput);
            //            //OnSprint?.Invoke(inputs.SprintInput);
            //        }
            //        break;
            //}
            #endregion
        }
        #endregion
    }
}