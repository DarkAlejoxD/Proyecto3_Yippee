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

        private PlayerController _playerController;


        
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

            _playerController.OnMovement += OnMovement
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
        private void OnMovement(Vector2 moveInput, PlayerMovementData movementData)
        {

        }
        #endregion
    }
}

