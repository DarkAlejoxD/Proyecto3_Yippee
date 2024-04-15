using UnityEngine;

namespace AvatarController //add it to a concrete namespace
{
    [RequireComponent(typeof(PlayerController))] //Add this if necessary, delete it otherwise
    public class PlayerGhostView : MonoBehaviour
    {
        #region Fields
        [Header("A")]
        [SerializeField] private float _privateAttribute;
        public int PublicAttribute;
        #endregion

        #region Unity Logic
        private void Awake()
        {
        }

        private void Update()
        {
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