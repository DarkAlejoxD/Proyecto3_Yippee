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
        private const string ANY_CONST = "";

        [Header("Section1")]
        [SerializeField] private float _privateAttribute;
        public int PublicAttribute;

        public float Property => _privateAttribute * PublicAttribute;

        [Header("Section2")]
        private float _attribute2;
        #endregion

        #region Static Methods
        public static void StaticMethod()
        {
        }
        #endregion

        #region Unity Logic
        private void Awake()
        {
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
        private void PrivateMethod()
        {
        }
        #endregion
    }
}

