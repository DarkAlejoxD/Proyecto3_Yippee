using UnityEngine;

namespace Mechanism
{
    [DisallowMultipleComponent]
    public abstract class AbsMechanism : MonoBehaviour
    {
        #region UnityLogic
        private void Awake()
        {
            //Temporal
            GetComponentInParent<AbsInterruptor>().Activate += Activate;
            GetComponentInParent<AbsInterruptor>().Deactivate += Deactivate;
        }

        private void Start()
        {            
            Deactivate();
        }

        private void OnDestroy()
        {
            //Temporal
            GetComponentInParent<AbsInterruptor>().Activate -= Activate;
            GetComponentInParent<AbsInterruptor>().Deactivate -= Deactivate;
        }
        #endregion

        #region Public Methods
        public abstract void Activate();
        public virtual void Deactivate()
        {
            Debug.LogWarning("Not implemented");
        }
        #endregion
    }
}
