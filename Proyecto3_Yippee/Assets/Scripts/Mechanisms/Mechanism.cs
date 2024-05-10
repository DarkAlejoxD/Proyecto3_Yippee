using UnityEngine;
using UnityEngine.Events;
using BaseGame;

namespace Mechanisms
{
    [ExecuteAlways]
    public class Mechanism : MonoBehaviour, IMechanism, IResetable
    {
        private enum MechanismType
        {
            On_Off,
            ContinuousSignal,
            JustOn
        }

        [Header("References")]
        private KeyManager _keyManager;

        [Header("Events")]
        [SerializeField] private UnityEvent _activate;
        [SerializeField] private UnityEvent _deactivate;

        [Header("Type")]
        [SerializeField] private MechanismType _mechanismType = MechanismType.JustOn;
        private bool _isActive;
        private bool _lastSignal;

        public bool IsActive => _isActive;

        #region Unity Logic
        private void Awake()
        {
            _isActive = false;
            _lastSignal = false;
            _deactivate?.Invoke();
        }

        private void OnEnable()
        {
            _keyManager = GetComponentInParent<KeyManager>();
            _keyManager.AddToMechanismList(this);
        }

        private void OnDisable()
        {
            _keyManager.RemoveFromMechanismList(this);
        }

        public void Reset()
        {
            _isActive = false;
            _deactivate?.Invoke();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Techinally, this will be called when an interruptor suffer a change of its state, so...
        /// </summary>
        /// <param name="signal"></param>
        public void ReceiveSignal(bool signal)
        {
            switch (_mechanismType)
            {
                case MechanismType.On_Off:
                    {
                        if (!_lastSignal && signal)
                        {
                            if (_isActive)
                            {
                                _deactivate?.Invoke();
                                _isActive = false;
                            }
                            else
                            {
                                _activate?.Invoke();
                                _isActive = true;
                            }
                        }
                        _lastSignal = signal;
                    }
                    break;
                case MechanismType.ContinuousSignal:
                    {
                        if (_isActive)
                        {
                            if (signal) // Unexpected case, but...
                                return;

                            _deactivate?.Invoke();
                            _isActive = false;
                        }
                        else
                        {
                            if (!signal)
                                return;

                            _activate?.Invoke();
                            _isActive = true;
                        }
                    }
                    break;
                case MechanismType.JustOn:
                    {
                        if (_isActive)
                            return;

                        if (!signal)
                            return;

                        _activate?.Invoke();
                        _isActive = true;
                    }
                    break;
            }
        }
        #endregion
    }
}
