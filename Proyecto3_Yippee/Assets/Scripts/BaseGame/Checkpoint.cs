using UnityEngine;

namespace BaseGame
{    
    public class Checkpoint : MonoBehaviour
    {
        #region Fields
        [Header("Section1")]
        [SerializeField] private float _privateAttribute;
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
