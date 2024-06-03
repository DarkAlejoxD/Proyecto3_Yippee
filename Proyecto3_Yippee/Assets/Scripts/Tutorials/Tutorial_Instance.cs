using UnityEngine;
using UtilsComplements;

namespace Tutorials
{    
    public class Tutorial_Instance : MonoBehaviour
    {
        #region Fields
        [Header("Tutorial")]
        [SerializeField] private GameObject _keyboardRef;
        [SerializeField] private GameObject _controllerRef;                                                                                                     
        #endregion    

        #region Unity Logic
        private void Awake()
        {                
            _keyboardRef.SetActive(false);
            _controllerRef.SetActive(false);
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
