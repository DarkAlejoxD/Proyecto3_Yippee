using System;
using UnityEngine;
using AvatarController.Data;
using InputController;
using UtilsComplements;

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
        public Action<bool> OnInspect;
        public Action<bool> OnSprint;

        public PlayerData DataContainer => _dataContainer;
        public bool IsGrounded => _playerJump.IsGrounded;

        #region Unity Logic
        private void OnEnable()
        {
            if (!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected += OnGetInputs;

            OnInspect += (bool a) => { if(a)Debug.Log("Inspect"); };
        }

        private void OnDisable()
        {
            if (!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected -= OnGetInputs;
        }

        private void Awake()
        {
            ISingleton<GameManager>.GetInstance().SetPlayerInstance(this);
            _playerMovement = GetComponent<PlayerMovement>();
            _playerJump = GetComponent<PlayerJump>();
            _characterController = GetComponent<CharacterController>();
            _playerDive = GetComponent<PlayerDive>();
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

        #endregion

        #region Private Methods
        private void OnGetInputs(InputValues inputs) //TODO: Separate States
        {
            OnMovement?.Invoke(inputs.MoveInput);
            OnJump?.Invoke(inputs.JumpInput);
            OnDive?.Invoke(inputs.CrounchDiveInput);
            OnInteract?.Invoke(inputs.InteractInput);
            OnInspect?.Invoke(inputs.GhostViewInput);
            OnSprint?.Invoke(inputs.SprintInput);
        }
        #endregion
    }
}