using AvatarController;
using AvatarController.Data;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace AvatarController.Interaction 
{
    [RequireComponent(typeof(PlayerController), typeof(CharacterController))]
    public class PlayerInteractor : MonoBehaviour
    {
        #region Fields
        private PlayerController _playerController;
        private PlayerData Data => _playerController.DataContainer;
        #endregion

        #region Unity Logic
        private void OnEnable()
        {
            if (_playerController == null)
                _playerController = GetComponent<PlayerController>();

            _playerController.OnInteract += OnInteract;
        }

        private void OnDisable()
        {
            if (_playerController == null)
                _playerController = GetComponent<PlayerController>();

            _playerController.OnInteract -= OnInteract;
        }

        private void Awake()
        {    
            _playerController = GetComponent<PlayerController>();
        }

        private void Update()
        {      
          
        }
        #endregion


        #region Public Methods
        public void PublicMethod()
        {

        }
        #endregion

        #region Private Methods
        private void OnInteract(bool active)
        {
            if (!active) return;

            //TODO: CAST SPHERE
            //TODO: CHECK INTERACTABLE
            //TODO: MAKE INTERACTION
            Debug.Log("Interaction Input");

        }
        #endregion
    }
}
