using System;
using UtilsComplements;
using UnityEngine;

namespace InputController
{
    public class InputManager : MonoBehaviour, ISingleton<InputManager>
    {
        private PlayerMap _playerMap;

        public Action<Vector2> OnMove;
        public Action OnJump;
        public Action OnInteract;

        public ISingleton<InputManager> Instance => this;

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();

            _playerMap = new();
            _playerMap.PlayerMove.Enable();            
        }

        private void Update()
        {
            MoveUpdate();
            JumpUpdate();
            InteractUpdate();
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }
        #endregion

        #region Private Methods
        private void MoveUpdate()
        {
            Vector2 movement = _playerMap.PlayerMove.Move.ReadValue<Vector2>();
            if (movement.magnitude > 1) 
                movement.Normalize();

            OnMove?.Invoke(movement);
        }

        private void JumpUpdate()
        {
            float value = _playerMap.PlayerMove.Jump.ReadValue<float>();
            bool triggered = value > 0;

            if (triggered)
                OnJump?.Invoke();
        }

        private void InteractUpdate()
        {
            bool triggered = _playerMap.PlayerMove.Interact.WasReleasedThisFrame();

            if(triggered)
                OnInteract?.Invoke();
        }
        #endregion
    }
}
