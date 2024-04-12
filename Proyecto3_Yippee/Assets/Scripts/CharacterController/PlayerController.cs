using UnityEngine;

namespace CharacterController
{
    //[RequireComponent(typeof(Transform))] //Add this if necessary, delete it otherwise
    public class PlayerController : MonoBehaviour
    {
        //TODO: FSM or something that show every state and every action this state can be predecessor.
        #region Fields
        [Header("Data")]
        [SerializeField] private float _dataContainer;
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