using UnityEngine;
using UnityEngine.UI;
using UtilsComplements;

namespace BaseGame //add it to a concrete namespace
{
    public class DeadManager : MonoBehaviour, ISingleton<DeadManager>
    {
        #region Fields
        [Header("Section1")]
        [SerializeField] private Image _image;

        public ISingleton<DeadManager> Instance => this;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();
        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
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
