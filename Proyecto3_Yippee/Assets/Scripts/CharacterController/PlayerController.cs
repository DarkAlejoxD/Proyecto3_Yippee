using System;
using UnityEngine;
using InputController;
using FSM;
using AvatarController.Data;
using AvatarController.PlayerFSM;
using static UtilsComplements.AsyncTimer;

namespace AvatarController
{
    [SelectionBase]
    [RequireComponent(typeof(InputManager), typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        //TODO: Make this class control VelocityXY
        #region Fields
        [Header("Data")]
        [SerializeField] private PlayerData _dataContainer;
        private CharacterController _characterController;
        private InputManager _inputManager;

        public PlayerData DataContainer => _dataContainer;

        [Header("Delegates")]
        public Action<Vector2> OnMovement;
        public Action<bool> OnJump;
        public Action<bool> OnDive;
        public Action<bool> OnInteract;
        public Action<bool> OnGhostView;
        public Action<bool> OnSprint;

        [Header("Random Attributes")]
        //public bool isPushing = false;
        //private PlayerMovement _playerMovement;
        private PlayerJump _playerJump;
        //private PlayerDive _playerDive;

        //public bool IsGrounded => _playerJump.IsGrounded;

        [Header("CanTransitionValues")]
        private bool _canActivatePoltergeist;

        [Header("Velocity Attributes")]
        internal Vector3 Velocity;
        internal float VelocityY;
        private bool _useGravity;
        internal bool OnGround;

        internal float Gravity => Physics.gravity.y * DataContainer.DefaultJumpValues.GravityMultiplier;
        /// <summary> Meant to be the decceleration when the player released the button to reach the 
        /// peak of the jump earlier </summary>
        internal float TwistGravity { get; set; }
        internal bool UseTwikedGravity { get; set; }

        [Header("FSM")]
        private FSM_Player<PlayerStates> _playerFSM;

        public PlayerStates CurrentState => _playerFSM.CurrentState;
        public PlayerStates LastState => _playerFSM.LastState;

        [Header("DEBUG")]
        [SerializeField] TMPro.TMP_Text DEBUG_TextTest;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            GameManager.GetGameManager().SetPlayerInstance(this);
            _characterController = GetComponent<CharacterController>();
            _inputManager = GetComponent<InputManager>();

            _playerJump = GetComponent<PlayerJump>();
            //_playerMovement = GetComponent<PlayerMovement>();
            //_playerDive = GetComponent<PlayerDive>();
            UseTwikedGravity = false;
            TwistGravity = 0;

            FSMInit();
        }

        private void OnEnable()
        {
            if (_inputManager == null)
                _inputManager = GetComponent<InputManager>();

            _inputManager.OnInputDetected += OnGetInputs;
        }

        private void OnDisable()
        {
            if (_inputManager == null)
                _inputManager = GetComponent<InputManager>();

            _inputManager.OnInputDetected -= OnGetInputs;
        }

        private void Start()
        {
            Velocity = Vector3.zero;
            VelocityY = 0;
            _useGravity = true;
            _canActivatePoltergeist = true;
            if (DEBUG_TextTest)
                DEBUG_TextTest.text = "";
        }

        private void Update()
        {
            UpdateVy();

#if UNITY_EDITOR
            if (!DEBUG_TextTest)
                return;
            if (!_playerFSM.Equals(null))
                DEBUG_TextTest.text = "Current State: " + _playerFSM.Name;
#endif
        }
        #endregion

        #region Public Methods
        public void BlockMovement() => _characterController.enabled = false;

        public void UnBlockMovement() => _characterController.enabled = true;

        /// <summary></summary>
        /// <param name="dir"> the direction the player should look at when block</param>
        public void BlockMovement(Vector3 dir)
        {
            BlockMovement();

            Quaternion desiredRotation = Quaternion.LookRotation(dir);
            transform.rotation = desiredRotation;
        }

        public void RequestTeleport(Vector3 position)
        {
            BlockMovement();
            StopVelocity();
            StopFalling();
            transform.position = position;
            UnBlockMovement();
        }

        public void RequestTeleport(Vector3 position, Vector3 forward)
        {
            BlockMovement(forward);
            StopVelocity();
            StopFalling();
            transform.position = position;
            UnBlockMovement();
        }

        public void StopVelocity() => Velocity = Vector3.zero;

        public void StopFalling() => VelocityY = 0;

        public void SetGravityActive(bool state)
        {
            _useGravity = state;
            if (!state) StopVelocity();
        }

        internal void ForceChangeState(PlayerStates state) => _playerFSM.ForceChange(state);

        internal void RequestChangeState(PlayerStates state)
        {
            if (state == PlayerStates.OnPoltergeist)
            {
                if (!_canActivatePoltergeist)
                {
                    return;
                }
            }
            _playerFSM.RequestChange(state);
        }

        internal void ReturnState() => _playerFSM.ReturnLastState();

        internal void StartPoltergeist()
        {
            _inputManager.SetPlayerMapActive(false);
            _inputManager.SetPoltergeistActive(true);
        }

        internal void EndPoltergeist()
        {
            _inputManager.SetPlayerMapActive(true);
            _inputManager.SetPoltergeistActive(false);

            _canActivatePoltergeist = false;
            StartCoroutine(TimerCoroutine(_dataContainer.DefPoltValues.PoltergeistCD,
                                          () => _canActivatePoltergeist = true));
        }
        #endregion

        #region Private Methods
        private void FSMInit()
        {
            _playerFSM = new();

            _playerFSM.SetRoot(PlayerStates.OnGround, new PlayerState_DefaultMovement(this));
            _playerFSM.AddState(PlayerStates.OnAir, new PlayerState_OnAir(this));
            _playerFSM.AddState(PlayerStates.Grabbing, new PlayerState_Grabbing(this));
            _playerFSM.AddState(PlayerStates.OnDive, new PlayerState_OnDive(this));
            _playerFSM.AddState(PlayerStates.OnPoltergeist, new PlayerState_Poltergeist(this));
            _playerFSM.AddState(PlayerStates.Jumping, new PlayerState_Jumping(this));

            Transition toAir = new(() =>
            {
                return !_playerJump.CanJump();
            });

            Transition grounded = new(() =>
            {
                return OnGround;
            });

            _playerFSM.AddAutoTransition(PlayerStates.OnGround, toAir, PlayerStates.OnAir);
            _playerFSM.AddAutoTransition(PlayerStates.OnAir, grounded, PlayerStates.OnGround);

            //Manual Transitions should be named here:
            // - When grab a GrabLedge and LetGoLedge
            // - Dive enter and exit
            // - OnPoltergeistEnter --> OnGroundState
            // - OnPoltergeistExit --> OnPoltergeist
            // - Jump() in PlayerJump --> Jumping
            // - in Jumping_State --> OnAir

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

        private void UpdateVy()
        {
            if (!_useGravity) return;

            float variation = VelocityY * Time.deltaTime;

            CollisionFlags movement = _characterController.Move(new Vector3(0, variation, 0) *
                                                                DataContainer.DefOtherValues.ScaleMultiplicator);

            if (movement == (CollisionFlags.Above))
                VelocityY = 0;

            if (movement == CollisionFlags.Below)
            {
                OnGround = true;
                VelocityY = 0;
            }
            else
            {
                OnGround = false;
            }

            if (VelocityY < 0)
            {
                VelocityY += Gravity * Time.deltaTime * DataContainer.DefaultJumpValues.DownGravityMultiplier;
                UseTwikedGravity = false;
            }
            else
            {
                if (UseTwikedGravity)
                    VelocityY += TwistGravity * Time.deltaTime;
                else
                    VelocityY += Gravity * Time.deltaTime;
            }
        }
        #endregion
    }
}