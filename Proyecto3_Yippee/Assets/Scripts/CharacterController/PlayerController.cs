using System;
using UnityEngine;
using AvatarController.Data;
using InputController;
using UtilsComplements;
using AvatarController.PlayerFSM;

namespace AvatarController
{
    [RequireComponent(typeof(InputManager), typeof(CharacterController))] //Add this if necessary, delete it otherwise
    public class PlayerController : MonoBehaviour
    {
        //Backlog: FSM or something that show every state and every action this state can be predecessor.????

        //TODO: 
        #region Fields
        [Header("Data")]
        [SerializeField] private PlayerData _dataContainer;
        private CharacterController _characterController;

        public bool isPushing = false;
        private PlayerMovement _playerMovement;
        private PlayerJump _playerJump;
        private PlayerDive _playerDive;
        #endregion

        public Action<Vector2> OnMovement; //Vector2 --> direction
        public Action<bool> OnJump;
        public Action<bool> OnDive;
        public Action<bool> OnInteract;
        public Action<bool> OnGhostView;
        public Action<bool> OnSprint;
        public Action OnPoltergeistEnter;
        public Action<Vector2, float> OnPoltergeistStay;
        public Action<bool> OnPoltergeistExit;

        private PlayerStates _currentState;
        //private Dictionary<PlayerStates, PlayerStateForFSM> _playerBrain;

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
        }

        private void Start()
        {
            FSMInit();
        }

        private void FSMInit()
        {
            //_playerBrain = new Dictionary<PlayerStates, PlayerStateForFSM>();
            //PlayerStateForFSM onGround = new(PlayerStates.OnGround, new PlayerStates[]
            //{
            //    PlayerStates.OnPoltergeist,
            //    PlayerStates.OnDive
            //});
            //PlayerStateForFSM onDive = new(PlayerStates.OnDive, PlayerStates.OnGround);
            //PlayerStateForFSM onPoltergeist = new(PlayerStates.OnPoltergeist, PlayerStates.OnGround);

            //_playerBrain.Add(PlayerStates.OnGround, onGround);
            //_playerBrain.Add(PlayerStates.OnDive, onDive);
            //_playerBrain.Add(PlayerStates.OnPoltergeist, onPoltergeist);
        }

        private void OnEnable()
        {
            if (!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected += OnGetInputs;

            //OnInspect += (bool a) => { if(a)Debug.Log("Inspect"); };
        }

        private void OnDisable()
        {
            if (!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected -= OnGetInputs;
        }
        #endregion

        #region Public Methods

        //For PROTO, change when FSM
        public void EnablePushingMode(Vector3 dir)
        {
            _characterController.enabled = false;
            _playerDive.enabled = false;
            _playerJump.enabled = false;

            Quaternion desiredRotation = Quaternion.LookRotation(dir);
            transform.rotation = desiredRotation;
            _playerMovement.StopVelocity();
            _playerMovement.enabled = false;
        }

        public void DisablePushingMode()
        {
            _characterController.enabled = true;
            _playerDive.enabled = true;
            _playerJump.enabled = true;
            _playerMovement.enabled = true;
            _playerMovement.StopVelocity();

        }

        #region Proto FSM
        public void RequestTeleport(Vector3 position)
        {
            EnablePushingMode(transform.forward);//hardcoded maybe?
            transform.position = position;
            DisablePushingMode();
        }
        #endregion

        #endregion

        #region Private Methods
        private void OnGetInputs(InputValues inputs) //TODO: Separate States
        {
            switch (_currentState)
            {
                case PlayerStates.OnPoltergeist:
                    {
                        float upDown = 0;
                        if (inputs.JumpInput) upDown += 1;
                        if (inputs.CrounchDiveInput) upDown -= 1;
                        OnPoltergeistStay?.Invoke(inputs.MoveInput, upDown);
                        OnPoltergeistExit?.Invoke(inputs.CancelInput);
                    }
                    break;
                case PlayerStates.OnDive:
                    break;
                default:
                    {
                        OnMovement?.Invoke(inputs.MoveInput);
                        OnJump?.Invoke(inputs.JumpInput);
                        OnDive?.Invoke(inputs.CrounchDiveInput);
                        OnInteract?.Invoke(inputs.InteractInput);
                        OnGhostView?.Invoke(inputs.GhostViewInput);
                        //OnSprint?.Invoke(inputs.SprintInput);
                    }
                    break;
            }
        }
        #endregion
    }
}