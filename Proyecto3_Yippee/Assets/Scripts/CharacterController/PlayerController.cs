using System;
using UnityEngine;
using CharacterController.Data;
using InputController;
using UtilsComplements;

namespace CharacterController
{
    //[RequireComponent(typeof(InputManager))] //Add this if necessary, delete it otherwise
    public class PlayerController : MonoBehaviour
    {
        //Backlog: FSM or something that show every state and every action this state can be predecessor.????

        //TODO: 
        #region Fields
        [Header("Data")]
        [SerializeField] private PlayerData _dataContainer;
        public int PublicAttribute;
        #endregion

        public delegate void MovementDelegate(Vector2 direction, PlayerMovementData data);
        public MovementDelegate OnMovement;

        #region Unity Logic
        private void OnEnable()
        {
            if(!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected += OnGetInputs;
        }

        private void OnDisable()
        {
            if (!ISingleton<InputManager>.TryGetInstance(out var inputManager))
                return;

            inputManager.OnInputDetected -= OnGetInputs;
        }

        private void OnGetInputs(InputValues inputs)
        {
            OnMovement?.Invoke(inputs.MoveInput, _dataContainer.DefaultMovement);
        }
        #endregion

        #region Static Methods
        public static void StaticMethod()
        {
        }
        #endregion

        #region Public Methods
        public void PublicMethod()
        {
        }
        #endregion

        #region Private Methods
        private void PrivateMethod()
        {
        }
        #endregion
    }
}