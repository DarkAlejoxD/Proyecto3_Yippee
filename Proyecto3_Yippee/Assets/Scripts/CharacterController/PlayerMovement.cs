using UnityEngine;
using InputController;

namespace CharacterController
{
    [RequireComponent(typeof(InputManager))] //Add this if necessary, delete it otherwise
    public class PlayerMovement : MonoBehaviour
    {
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
