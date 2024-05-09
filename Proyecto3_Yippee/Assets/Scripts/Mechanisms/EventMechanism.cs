using UnityEngine;
using UnityEngine.Events;

namespace Mechanism
{
    public class EventMechanism : AbsMechanism
    {
        #region Fields
        [Header("Events")]
        [SerializeField] private UnityEvent _enableEvent;
        [SerializeField] private UnityEvent _disableEvent;
        #endregion        

        #region Public Methods
        public override void Activate() => _enableEvent?.Invoke();
        public override void Deactivate() => _disableEvent?.Invoke();
        #endregion
    }
}
