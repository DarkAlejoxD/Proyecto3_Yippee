using UnityEngine;
using InputController;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerJump : MonoBehaviour
    {
        //TODO: Get Design Specifications: For proto regular jump, for >=alpha mantein to higher
        //TODO: Make this class.

        #region Fields
        private PlayerController _controller;
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
        private void OnJump(bool active)
        {
        }
        #endregion
    }
}

