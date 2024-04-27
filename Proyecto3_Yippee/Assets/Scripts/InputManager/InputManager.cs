using System;
using UtilsComplements;
using UnityEngine;

namespace InputController
{
    public class InputManager : MonoBehaviour, ISingleton<InputManager>
    {
        public Action<InputValues> OnInputDetected;

        private PlayerMap _playerMap;
        private InputValues _inputValues;

        public ISingleton<InputManager> Instance => this;

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();

            _playerMap = new();
            _playerMap.PlayerMove.Enable();
            _inputValues = new InputValues();
        }

        private void Update()
        {
            //Read Inputs
            _inputValues.ResetInputs();
            MoveUpdate();
            JumpUpdate();
            CrounchDive();
            InteractUpdate();
            GhostViewUpdate();
            SprintUpdate();
            CancelUpdate();

            //Send Inputs
            OnInputDetected?.Invoke(_inputValues);
        }

        private void OnDestroy() => Instance.RemoveInstance();
        #endregion

        #region Private Methods
        private void MoveUpdate()
        {
            Vector2 movement = _playerMap.PlayerMove.Move.ReadValue<Vector2>();
            if (movement.magnitude > 1)
                movement.Normalize();

            _inputValues.MoveInput = movement;
        }

        private void JumpUpdate()
        {
            float value = _playerMap.PlayerMove.Jump.ReadValue<float>();
            _inputValues.JumpInput = value > 0;
        }

        private void InteractUpdate()
        {
            bool triggered = _playerMap.PlayerMove.Interact.WasReleasedThisFrame();
            _inputValues.InteractInput = triggered;
        }

        private void GhostViewUpdate()
        {
            bool triggered = _playerMap.PlayerMove.SurvivalInstinct.WasReleasedThisFrame();
            _inputValues.GhostViewInput = triggered;
        }

        private void CrounchDive()
        {
            bool triggered = _playerMap.PlayerMove.CrouchDive.IsPressed();
            _inputValues.CrounchDiveInput = triggered;
        }

        private void SprintUpdate()
        {
            bool triggered = _playerMap.PlayerMove.Sprint.IsPressed();
            _inputValues.SprintInput = triggered;
        }

        private void CancelUpdate()
        {
            bool triggered = _playerMap.PlayerMove.Cancel.WasReleasedThisFrame();
            _inputValues.CancelInput = triggered;
        }
        #endregion
    }
}
